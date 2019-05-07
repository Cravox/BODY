using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class WalkingW : SerializedMonoBehaviour
{
    [BoxGroup("Requirements"), Required]
    [InfoBox("Objects are required to run.")]
    public Rigidbody rigid;
    [BoxGroup("Requirements"), Required]
    public Camera cam;

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
    private Vector3 moveForce;  //used for movement direction
    private Vector3 lastForce;  //used for rotation stance
    [BoxGroup("Stats"), SerializeField]
    private bool inputJump;
    [BoxGroup("Stats"), SerializeField]
    private bool isGrounded;

    private Vector3 savedVelocity;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigid.AddForce(Physics.gravity * 4);
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
            rigid.velocity = new Vector3(moveForce.x * walkSpeed, rigid.velocity.y, moveForce.z * walkSpeed);
            savedVelocity = new Vector3(rigid.velocity.x, 0, rigid.velocity.z) - (moveForce * airSpeed);
        }
        else
            rigid.velocity = new Vector3(0, rigid.velocity.y, 0) + savedVelocity + (moveForce * airSpeed);
        //prevent model from returning rotation to zero
        if (moveForce != Vector3.zero && isGrounded)
            lastForce = moveForce;

        //rotate model to moving direction (which is relative to the camera)
        float modelRot = (Vector3.SignedAngle(lastForce, transform.forward, -Vector3.up));
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
}
