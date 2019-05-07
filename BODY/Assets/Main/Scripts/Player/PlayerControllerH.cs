using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerControllerH : MonoBehaviour {
    // how high the player can jump
    [Range(10, 100), TabGroup("Balancing")]
    public float jumpForce;

    // how fast the player can move
    [SerializeField, Range(1, 100), TabGroup("Balancing")]
    private float movementSpeed;

    [SerializeField, Range(1, 100), TabGroup("Balancing")]
    private float gravityScale;

    // The players attached CharacterController
    [Required, SerializeField, TabGroup("References"), Tooltip("References the players attached CharacterController")]
    private CharacterController characterController;

    [Required, SerializeField, TabGroup("References"), Tooltip("References the scenes main camera")]
    private Transform camTrans;

    [SerializeField, TabGroup("Settings"), Tooltip("How fast the character turns")]
    private float rotationSmooth = 0.5f;

    [SerializeField, TabGroup("References")]
    private Animator anim;

    [Range(1,2), SerializeField, TabGroup("Balancing")]
    private float airborneSlow;

    private Vector3 moveDir;

    Vector2 input;
    private Vector3 lastLook;

    // Update is called once per frame
    void Update()
    {
        Movement();
        anim.SetFloat("Velocity", characterController.velocity.magnitude / movementSpeed);
        anim.SetBool("IsGrounded", characterController.isGrounded);
    }

    void Movement()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        var yStore = moveDir.y;
        var camFwd = camTrans.transform.forward;
        var camRight = camTrans.transform.right;

        camFwd.y = 0f;
        camRight.y = 0f;

        camFwd.Normalize();
        camRight.Normalize();

        moveDir = (camFwd * -input.y * movementSpeed) + (camRight * input.x * movementSpeed);
        moveDir.y = yStore;

        lastLook = new Vector3(moveDir.x, 0, moveDir.z);

        if (characterController.isGrounded)
        {
            moveDir.y = 0f;
            if (Input.GetButtonDown("Jump")) {
                moveDir.y = jumpForce;
                anim.Play("Jump");
            }
        }

        moveDir = characterController.isGrounded ? moveDir : new Vector3(moveDir.x/airborneSlow, moveDir.y, moveDir.z/airborneSlow);

        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lastLook), rotationSmooth);
        }

        moveDir.y = moveDir.y + (Physics.gravity.y * gravityScale * Time.deltaTime);

        characterController.Move(moveDir * Time.deltaTime);
    }
}