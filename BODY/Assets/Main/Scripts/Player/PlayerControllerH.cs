using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerControllerH : MonoBehaviour {
    // how high the player can jump
    [Range(10, 30), TabGroup("Balancing")]
    public float jumpForce;

    // how fast the player can move
    [SerializeField, Range(1, 10), TabGroup("Balancing")]
    private float movementSpeed;

    [SerializeField, Range(1, 5), TabGroup("Balancing")]
    private float gravityScale;

    // The players attached CharacterController
    [Required, SerializeField, TabGroup("References"), Tooltip("References the players attached CharacterController")]
    private CharacterController characterController;
    
    private Vector3 moveDir;

    // Update is called once per frame
    void Update() {
        var yStore = moveDir.y;
        moveDir = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * -Input.GetAxis("Horizontal"));
        moveDir = moveDir.normalized * -movementSpeed;
        moveDir.y = yStore;

        if (characterController.isGrounded) {
            moveDir.y = 0f;
            if (Input.GetButtonDown("Jump")) moveDir.y = jumpForce;
        }



        moveDir.y = moveDir.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        
        characterController.Move(moveDir * Time.deltaTime);
    }
}
