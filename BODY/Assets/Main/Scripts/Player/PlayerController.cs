using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class PlayerController : SerializedMonoBehaviour
{
    public static PlayerController instance;

    [TabGroup("Balancing")]
    public float gravity;
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
    [TabGroup("References")]
    public Transform groundHolder;
    [TabGroup("References")]
    public LayerMask checkCollisionOn;
    [TabGroup("References"), InfoBox("Required for turning a Model during Movement"), Header("Model")]
    public Transform modelAxis;
    [TabGroup("References")]
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
    public bool stopGravity;
    [TabGroup("Debugging"), SerializeField]
    public Vector3 savedVelocity;
    [TabGroup("Debugging"), SerializeField]
    public Rigidbody platform;
    [TabGroup("Debugging"), SerializeField]
    public List<PlayerForce> forces;
    [TabGroup("Debugging"), SerializeField]
    public Vector3 extraForce;

    public bool doubleJumped;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Forces();
        InputCheck();
        Move();
        GroundCheck();
        Animate();
    }

    void Animate()
    {
        Vector3 rigidvel = rigid.velocity - (platform != null ? platform.velocity : Vector3.zero);
        modelAnim.SetFloat("Velocity", rigidvel.magnitude / walkSpeed);
        modelAnim.SetBool("IsGrounded", isGrounded);
    }

    void InputCheck()
    {
        inputAxis = Vector2.MoveTowards(inputAxis, new Vector2(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical")), 9 * Time.deltaTime);
    }

    void Move()
    {
        //Add gravitational force
        if (!stopGravity)
            rigid.AddForce(new Vector3(0, -gravity, 0));
        else
            rigid.velocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z) + extraForce;

        //set direction vector of camera look rotation
        Vector3 camFwd = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        Vector3 camRight = new Vector3(cam.transform.right.x, 0, cam.transform.right.z);

        //normalize the vectors (so that diagonal movement is not faster than regular)
        camFwd.Normalize();
        camRight.Normalize();

        //combine cam direction with input axis amount
        moveForce = camRight * inputAxis.x + camFwd * inputAxis.y;

        //set velocity including speed to rigidbody, is velocity forward?
        if (isGrounded)
        {
            rigid.velocity = new Vector3(moveForce.x * walkSpeed, rigid.velocity.y, moveForce.z * walkSpeed) + extraForce + (platform != null ? platform.velocity : Vector3.zero);
            savedVelocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z) - (moveForce * airSpeed);
        }
        else
        {
            rigid.velocity = new Vector3(0, rigid.velocity.y, 0) + savedVelocity + (moveForce * airSpeed) + extraForce;
        }

        //prevent model from returning rotation to zero
        Vector3 walkVel = new Vector3(rigid.velocity.x, 0 , rigid.velocity.z) - (platform != null ? platform.velocity : Vector3.zero);
        if (walkVel.magnitude != 0)
            lastForce = rigid.velocity - (platform != null ? platform.velocity : Vector3.zero);

        //rotate model to moving direction (which is relative to the camera)
        float modelRot = Mathf.Atan2(lastForce.x, lastForce.z) * Mathf.Rad2Deg;
        modelAxis.localRotation = Quaternion.RotateTowards(modelAxis.localRotation, Quaternion.Euler(new Vector3(0, modelRot, 0)), 20f);
    }

    public void Jump()
    {
        rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        modelAnim.Play("Jump");
    }

    void GroundCheck()
    {
        isGrounded = (Physics.OverlapSphere(groundHolder.position, groundHoldRadius, checkCollisionOn).Length > 0);
        if (isGrounded) {
            doubleJumped = false;
        }
    }

    public void AddForce(Vector3 targetForce, float decay)
    {
        forces.Add(new PlayerForce(targetForce, decay));
    }

    public void AddForce(Vector3 targetForce, float decay, out PlayerForce pf)
    {
        pf = new PlayerForce(targetForce, decay);
        forces.Add(pf);
    }

    void Forces()
    {
        if (forces == null)
            return;

        Vector3 eF = new Vector3();
        
        foreach(PlayerForce pf in forces)
        {
            pf.strenght = Mathf.MoveTowards(pf.strenght, 0, (1 / pf.decay) * Time.deltaTime);

            pf.force = pf.initialForce * pf.strenght;

            if (pf.finished)
                pf.Stop();
            else
                eF += pf.force;
        }

        extraForce = eF;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "MovingPlatform")
        {
            platform = col.gameObject.GetComponentInParent<Rigidbody>();
        }
    }
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "MovingPlatform")
        {
            platform = null;
        }
    }
}

[System.Serializable]
public class PlayerForce
{
    public Vector3 initialForce;
    public Vector3 force;
    public float decay;
    public float strenght = 1f;
    public bool finished { get { return (this.force == Vector3.zero); } }

    public PlayerForce(Vector3 force, float decay)
    {
        this.initialForce = force;
        this.force = force;
        this.decay = decay;
    }

    public void Stop()
    {
        strenght = 0;
        force = Vector3.zero;
    }
}