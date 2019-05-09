using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class PlayerController : SerializedMonoBehaviour
{
    public static PlayerController instance;

    [BoxGroup("Requirements"), Required]
    [InfoBox("Objects are required to run.")]
    public Rigidbody rigid;
    [BoxGroup("Requirements"), Required]
    public Camera cam;

    [TabGroup("Movement")]
    public float gravity;
    [TabGroup("Movement")]
    public float walkSpeed;
    [TabGroup("Movement")]
    public float strafeSpeed;
    [TabGroup("Movement")]
    public float jumpSpeed;
    [TabGroup("Movement")]
    public float airSpeed;
    [TabGroup("Ground")]
    public float groundHoldRadius;
    [TabGroup("Ground")]
    public Transform groundHolder;
    [TabGroup("Ground")]
    public LayerMask checkCollisionOn;
    [TabGroup("Model"), InfoBox("Required for turning a Model during Movement")]
    public Transform modelAxis;
    [TabGroup("Model")]
    public Animator modelAnim;
 
    private Vector2 inputAxis;
    [InfoBox("Do not change these values, they are set automatically.")]
    [BoxGroup("Stats"), SerializeField]
    public Vector3 moveForce;  //used for movement direction
    private Vector3 lastForce;  //used for rotation stance
    [BoxGroup("Stats"), SerializeField]
    private bool inputJump;
    [BoxGroup("Stats"), SerializeField]
    public bool isGrounded;
    [BoxGroup("Stats"), SerializeField]
    public bool stopGravity;
    [BoxGroup("Stats"), SerializeField]
    public Vector3 savedVelocity;

    public List<PlayerForce> forces;
    public Vector3 extraForce;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        Forces();
        InputCheck();
        Jump();
        Move();
        GroundCheck();
        Animate();
    }

    void Animate()
    {
        modelAnim.SetFloat("Velocity", rigid.velocity.magnitude / walkSpeed);
        modelAnim.SetBool("IsGrounded", isGrounded);
    }

    void InputCheck()
    {
        inputAxis = Vector2.MoveTowards(inputAxis, new Vector2(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical")), 9 * Time.deltaTime);
        inputJump = Input.GetButtonDown("Jump");
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
            rigid.velocity = new Vector3(moveForce.x * walkSpeed, rigid.velocity.y, moveForce.z * walkSpeed) + extraForce;
            savedVelocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z) - (moveForce * airSpeed);
        }
        else
        {
            rigid.velocity = new Vector3(0, rigid.velocity.y, 0) + savedVelocity + (moveForce * airSpeed) + extraForce;
        }
        //prevent model from returning rotation to zero
        Vector3 walkVel = new Vector3(rigid.velocity.x, 0 , rigid.velocity.z);
        if (walkVel.magnitude != 0)
            lastForce = rigid.velocity;

        //rotate model to moving direction (which is relative to the camera)
        float modelRot = Mathf.Atan2(lastForce.x, lastForce.z) * Mathf.Rad2Deg;
        modelAxis.localRotation = Quaternion.RotateTowards(modelAxis.localRotation, Quaternion.Euler(new Vector3(0, modelRot, 0)), 20f);
    }

    void Jump()
    {
        if (inputJump && isGrounded)
        {
            rigid.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            modelAnim.Play("Jump");
        }
    }

    void GroundCheck()
    {
        isGrounded = (Physics.OverlapSphere(groundHolder.position, groundHoldRadius, checkCollisionOn).Length > 0); 
    }

    public void AddForce(Vector3 targetForce, float decay)
    {
        forces.Add(new PlayerForce(targetForce, decay));
    }

    public PlayerForce AddForceReturn(Vector3 targetForce, float decay)
    {
        PlayerForce pf = new PlayerForce(targetForce, decay);
        forces.Add(pf);
        return pf;
    }

    void Forces()
    {
        if (forces == null)
            return;

        Vector3 eF = new Vector3();
        
        foreach(PlayerForce pf in forces)
        {
            pf.force *= (1 - pf.decay);

            if (pf.finished)
                pf.Stop();
            else
                eF += pf.force;
        }

        extraForce = eF;
    }
}

[System.Serializable]
public class PlayerForce
{
    public Vector3 initialForce;
    public Vector3 force;
    public float decay;
    public bool finished { get { return (this.force == Vector3.zero); } }

    public PlayerForce(Vector3 force, float decay)
    {
        this.initialForce = force;
        this.force = force;
        this.decay = decay;
    }

    public void Stop()
    {
        force = Vector3.zero;
    }
}