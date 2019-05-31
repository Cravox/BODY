﻿using System.Collections;
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

    Vector3 platformNoY;

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
            rigid.AddForce(new Vector3(0, -gravity, 0) * Time.deltaTime);


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
        Vector3 walkVel = new Vector3(rigid.velocity.x, 0 , rigid.velocity.z) - platformNoY;
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
        Collider[] isGroundedOn = Physics.OverlapSphere(groundHolder.position, groundHoldRadius, checkCollisionOn);

        isGrounded = (isGroundedOn.Length > 0);

        if (isGrounded && isGroundedOn[0].gameObject.tag == "MovingPlatform")
            platform = isGroundedOn[0].gameObject.GetComponentInParent<Rigidbody>();
        else
            platform = null;
    }

    public void AddForce(Vector3 targetForce, float decay, bool isImpulse)
    {
        forces.Add(new PlayerForce(targetForce, decay, isImpulse));
    }

    public void AddForce(Vector3 targetForce, float decay, out PlayerForce pf, bool isImpulse)
    {
        pf = new PlayerForce(targetForce, decay, isImpulse);
        forces.Add(pf);
    }

    void Forces()
    {
        if (forces == null)
            return;

        //remove finished forces
        int forceAmount = forces.Count;
        for (int i = 0; i < forceAmount; i++)
        {
            if(forces[i].finished)
            {
                forces.RemoveAt(i);
                i--;
                forceAmount = forces.Count;
            }
        }

            Vector3 eF = new Vector3();
        
        //if force is impulse: decays after decay time
        //if force is not impulse: stops when decay time is reached

        foreach(PlayerForce pf in forces)
        {
            if (pf.isImpulse && pf.time > pf.decay)
                pf.strenght *= 0.6f;
            else
                pf.strenght = pf.time < pf.decay + 1 ? 1 : 0;

            pf.force = pf.initialForce * pf.strenght;

            if (pf.isImpulse && pf.strenght < 0.01f)
                pf.strenght = 0;

            if (pf.finished)
                pf.Stop();
            else
            {
                pf.time += Time.deltaTime;
                eF += pf.force;
            }
        }

        Vector3 platformVel = (platform != null ? platform.velocity : Vector3.zero);
        platformNoY = new Vector3(platformVel.x, 0, platformVel.z);

        eF += (platform != null ? platformNoY : Vector3.zero);

        extraForce = eF;
    }
}

[System.Serializable]
public class PlayerForce
{
    public Vector3 initialForce;
    public Vector3 force;
    public float decay;
    public float strenght = 1f;
    public float time = 1f;
    public bool isImpulse;
    public bool finished { get { return (this.force == Vector3.zero); } }

    public PlayerForce(Vector3 force, float decay, bool impulse)
    {
        this.initialForce = force;
        this.force = force;
        this.decay = decay;
        this.isImpulse = impulse;
    }

    public void Stop()
    {
        strenght = 0;
        force = Vector3.zero;
    }
}