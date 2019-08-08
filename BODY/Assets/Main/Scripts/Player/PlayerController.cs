using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class PlayerController : SerializedMonoBehaviour {
    [TabGroup("Balancing")]
    public float gravityModifier;

    [TabGroup("Balancing")]
    public float walkSpeed;

    [TabGroup("Balancing")]
    public float strafeSpeed;

    [TabGroup("Balancing")]
    public float jumpSpeed;

    [TabGroup("Balancing")]
    public float airSpeed;

    [TabGroup("References"), Required, Header("Requirements")]
    [InfoBox("Objects are required to run.")]
    public Rigidbody rigid;

    [TabGroup("References"), Required]
    public Camera cam;

    [TabGroup("References"), Header("Ground")]
    public float groundHoldRadius;

    [TabGroup("References"), Required]
    public Transform groundHolder;

    [TabGroup("References")]
    public LayerMask checkCollisionOn;

    [TabGroup("References"), InfoBox("Required for turning a Model during Movement"), Header("Model"), Required]
    public Transform modelAxis;

    [TabGroup("References"), Required]
    public Animator modelAnim;

    [TabGroup("Debugging"), SerializeField]
    [InfoBox("<color='red'><b>Debug only: Do not change these values. \nMay disrupt Gameplay when changed.</b></color>")]
    private Vector2 inputAxis;

    [TabGroup("Debugging"), SerializeField]
    public Vector3 moveForce;  //used for movement direction

    [TabGroup("Debugging"), SerializeField]
    private Vector3 lastForce;  //used for rotation stance

    [TabGroup("Debugging"), SerializeField]
    private bool inputJump;

    [TabGroup("Debugging"), SerializeField]
    public bool isGrounded;

    [TabGroup("Debugging"), SerializeField]
    public bool hovering;

    [TabGroup("Debugging"), SerializeField]
    public Rigidbody platform;

    [TabGroup("Debugging"), SerializeField]
    public List<PlayerForce> forces;

    [TabGroup("Debugging"), SerializeField]
    public Vector3 extraForce;

    [TabGroup("Debugging"), SerializeField]
    public float modelRot;

    [TabGroup("References"), Required]
    public CharAnimEvents animEvents;

    public Transform respawnTrans;

    private bool landed;

    Vector3 platformNoY;

    public bool stopGravity { get { return (hovering); } }

    void Awake() {

    }

    // Update is called once per frame
    void Update() {
        Forces();
        InputCheck();
        Move();
        GroundCheck();
        Animate();
    }

    private void FixedUpdate() {
        //Add gravitational force
        if (!stopGravity)
            rigid.AddForce(new Vector3(0, Physics.gravity.y * gravityModifier, 0));
    }

    void Animate() {
        Vector3 rigidvel = rigid.velocity - (platform != null ? platform.velocity : Vector3.zero);

        if(rigidvel.magnitude/walkSpeed >= 0.08f) {
            modelAnim.SetFloat("Velocity", rigidvel.magnitude / walkSpeed);
        } else {
            modelAnim.SetFloat("Velocity", 0);
        }
    }

    void InputCheck() {
        inputAxis = Vector2.MoveTowards(inputAxis, new Vector2(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical")), 9 * Time.deltaTime);

        if (Input.GetButtonDown("LeftBumper") && respawnTrans != null) {
            StartCoroutine(Dubstep());
        }
    }

    IEnumerator Dubstep() {
        GameManager.instance.fadeAnim.SetTrigger("Fade");
        yield return new WaitForSeconds(0.5f);
        this.transform.position = respawnTrans.position;
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.fadeAnim.SetTrigger("Fade");
    }

    void Move() {
        //set direction vector of camera look rotation
        Vector3 camFwd = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        Vector3 camRight = new Vector3(cam.transform.right.x, 0, cam.transform.right.z);

        //normalize the vectors (so that diagonal movement is not faster than regular)
        camFwd.Normalize();
        camRight.Normalize();

        //combine cam direction with input axis amount
        moveForce = camRight * inputAxis.x + camFwd * inputAxis.y;

        //set velocity including speed to rigidbody, is velocity forward?
        float currentSpeed = ((isGrounded || hovering) ? walkSpeed : airSpeed);
        rigid.velocity = new Vector3(moveForce.x * currentSpeed, rigid.velocity.y, moveForce.z * currentSpeed) + extraForce;

        //prevent model from returning rotation to zero
        Vector3 walkVel = new Vector3(rigid.velocity.x, 0, rigid.velocity.z) - platformNoY;
        if (walkVel.magnitude >= 0.1f)
            lastForce = rigid.velocity - (platform != null ? platform.velocity : Vector3.zero);

        //rotate model to moving direction (which is relative to the camera)
        modelRot = Mathf.Atan2(lastForce.x, lastForce.z) * Mathf.Rad2Deg;
        modelAxis.localRotation = Quaternion.RotateTowards(modelAxis.localRotation, Quaternion.Euler(new Vector3(0, modelRot, 0)), 20f);
    }

    public void JumpOnce(Vector3 jumpForce, bool regular, int forceId) {
        rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);

        Vector3 dirForceSide;

        if (regular)
            dirForceSide = transform.forward * jumpForce.z + transform.right * jumpForce.x;
        else
            dirForceSide = modelAxis.forward * jumpForce.z + transform.right * jumpForce.x;

        Vector3 dirForceUp = transform.up * jumpForce.y;

        AddForce(dirForceUp, 0, true, true);
        AddForce(dirForceSide, 50, true, true);
    }

    void GroundCheck() {
        Collider[] isGroundedOn = Physics.OverlapSphere(groundHolder.position, groundHoldRadius, checkCollisionOn);

        isGrounded = (isGroundedOn.Length > 0);

        string tag = "";

        if (isGrounded) {
            tag = isGroundedOn[0].gameObject.tag;
        } else {
            tag = "";
        }

        modelAnim.SetBool("isGrounded", isGrounded);

        if (tag == "MovingPlatform" || tag == "Carry")
            platform = isGroundedOn[0].gameObject.GetComponentInParent<Rigidbody>();
        else
            platform = null;
    }

    public void AddForce(int id, Vector3 targetForce, float decay, bool isImpulse, bool endOnGround) {
        forces.Add(new PlayerForce(id, targetForce, decay, isImpulse, endOnGround));
    }

    public void AddForce(Vector3 targetForce, float decay, bool isImpulse, bool endOnGround) {
        forces.Add(new PlayerForce(0, targetForce, decay, isImpulse, endOnGround));
    }
    public void StopAllForces() {
        foreach (PlayerForce pf in forces)
            pf.Stop();
    }

    public void StopForces(int id) {
        foreach (PlayerForce f in forces) {
            if (f.typeId == id)
                f.Stop();
        }
    }
    void Forces() {
        //do not apply any forces when empty
        if (forces == null)
            return;

        //remove finished forces
        int forceAmount = forces.Count;
        for (int i = 0; i < forceAmount; i++) {
            if (!forces[i].running) {
                forces.RemoveAt(i);
                i--;
                forceAmount = forces.Count;
            }
        }

        //create new combined force vector
        Vector3 eF = new Vector3();

        //force calculation for each force
        foreach (PlayerForce pf in forces) {
            //stop when on ground
            if (pf.cancelOnGround && isGrounded && pf.time > 0.5f)
                pf.Stop();

            //control the force strenght amount
            if (pf.time > pf.decay) {
                if (pf.isImpulse)           //if force is impulse: decays after decay time is reached
                    pf.strenght *= 0.6f;
                else
                    pf.strenght = 0;        //if force is not impulse: stops when decay time is reached
            }

            //apply strenght factor to initial force
            pf.force = pf.initialForce * pf.strenght;

            //if strenght is too low for measurable impact
            if (pf.isImpulse && pf.strenght < 0.01f)
                pf.strenght = 0;

            //stop when force time is finished
            if (pf.running) {
                pf.time += Time.fixedDeltaTime;
                eF += pf.force;
            } else
                pf.Stop();
        }

        //add platform force
        Vector3 platformVel = (platform != null ? platform.velocity : Vector3.zero);
        platformNoY = new Vector3(platformVel.x, 0, platformVel.z);
        eF += (platform != null ? platformNoY : Vector3.zero);

        //finally, set force
        extraForce = eF;
    }
}

[System.Serializable]
public class PlayerForce {
    public int typeId;
    public Vector3 initialForce;
    public Vector3 force;
    public float decay;
    public float strenght = 1f;
    public float time = 0;
    public bool isImpulse;
    public bool cancelOnGround;
    public bool running { get { return (this.force != Vector3.zero); } }

    public PlayerForce(int typeId, Vector3 force, float decay, bool impulse, bool cancelOnGround) {
        this.typeId = typeId;
        this.initialForce = force;
        this.force = force;
        this.decay = decay;
        this.isImpulse = impulse;
        this.cancelOnGround = cancelOnGround;
    }

    public void Stop() {
        strenght = 0;
        force = Vector3.zero;
    }
}