using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerControllerH : MonoBehaviour
{
    // The players attached CharacterController
    [Required, SerializeField, HideInPlayMode]
    private CharacterController characterController;

    [SerializeField, Required]
    private LegsH legs;

    private Vector3 moveDir;

    [SerializeField, Range(1, 5)]
    private float gravityScale;

    // how fast the player can move
    [SerializeField, Range(1, 10)]
    private float movementSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveDir = new Vector3(Input.GetAxis("Horizontal") * movementSpeed, moveDir.y, Input.GetAxis("Vertical") * movementSpeed);

        if (Input.GetButtonDown("Jump") && characterController.isGrounded) moveDir.y = legs.jumpForce;

        moveDir.y = moveDir.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        characterController.Move(moveDir * Time.deltaTime);
    }

    void Walk()
    {

    }
}
