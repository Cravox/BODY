using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Legs : Limb {
    [TabGroup("Balancing"), Header("Double Jump"), Tooltip("X = Sideways, Y = Up, Z = Forward")]
    public Vector3 dJumpSpeed;

    [TabGroup("Balancing"), Header("Wall Jump"), Tooltip("X = Sideways, Y = Up, Z = Forward")]
    public Vector3 wJumpSpeed;
    [TabGroup("Balancing")]
    public float wJumpDistanceToWall = 1f;

    [TabGroup("References")]
    public LayerMask rMask;

    [TabGroup("Debugging")]
    public bool jumping;
    [TabGroup("Debugging")]
    public bool doubleJumping;
    [TabGroup("Debugging")]
    public bool wallJumping;

    [TabGroup("Debugging")]
    public Transform wallRay;

    public bool hover;

    private bool wallJump { get { return Physics.OverlapSphere(wallRay.position, wJumpDistanceToWall, rMask).Length > 0; } }

    public override int TierOne() {
        if (!playerCont.isGrounded && !doubleJumping && !hover)
        {
            playerCont.JumpOnce(dJumpSpeed, false, 1);
            doubleJumping = true;
            return tierCosts[0];
        }
        else if (playerCont.isGrounded && !jumping && !hover) {
            Vector3 jumpSpeed = playerCont.moveForce * (playerCont.walkSpeed - playerCont.airSpeed) + new Vector3(0, playerCont.jumpSpeed, 0);
            playerCont.JumpOnce(jumpSpeed, true, 0);
            jumping = true;
        }

        return 0;
    }

    public override int TierTwo() {
        if (!playerCont.isGrounded && wallJump && !hover)
        {
            playerCont.StopAllForces(); //stop all forces to make walljump clean
            playerCont.JumpOnce(wJumpSpeed, false, 2);
            wallJumping = true;
            return tierCosts[1];
        }
        return 0;
    }

    public override int TierThree() {
        // hover

        if (!playerCont.isGrounded)
        {
            playerCont.StopAllForces();
            playerCont.rigid.velocity = Vector3.zero;
            hover = !hover;
        }

        return 0;

        //playerCont.modelAnim.SetBool("IsDashing", wallJumping);
    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {
        playerCont.modelAnim.SetBool("IsDashing", doubleJumping);

        if (playerCont.isGrounded) //if grounded, cancel ongoing forces
        {
                jumping = false;
                doubleJumping = false;
                wallJumping = false;
                hover = false;
        }

        playerCont.hovering = hover;

        playerCont.rigid.constraints = 
            (hover ? RigidbodyConstraints.FreezePositionY : RigidbodyConstraints.None) 
            | RigidbodyConstraints.FreezeRotationX 
            | RigidbodyConstraints.FreezeRotationY 
            | RigidbodyConstraints.FreezeRotationZ;
    }
}