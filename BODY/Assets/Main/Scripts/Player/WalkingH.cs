﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WalkingH : SerializedMonoBehaviour
{
    // The players attached CharacterController
    [Required, SerializeField, HideInPlayMode]
    private CharacterController characterController;

    [SerializeField, Required]
    private LegsH legs;

    private Vector3 moveDir;

    [SerializeField, Range(0.1f, 1)]
    private float gravityModifier;

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
        moveDir.y = moveDir.y + (Physics.gravity.y * gravityModifier * Time.deltaTime);
        if (Input.GetButtonDown("Jump") && characterController.isGrounded) moveDir.y = legs.jumpForce;
        characterController.Move(moveDir * Time.deltaTime);
    }

    void Walk()
    {

    }
}