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

    [TabGroup("Debugging")]
    public bool jumping;
    [TabGroup("Debugging")]
    public bool doubleJumping;
    [TabGroup("Debugging")]
    public bool wallJumping;

    [TabGroup("Debugging")]
    public Transform wallRay;
    public bool wallJump { get { return Physics.OverlapSphere(wallRay.position, wJumpDistanceToWall, rMask).Length > 0; } }
    public LayerMask rMask;
    public RaycastHit[] r;

    public override int TierOne() {
        if (playerCont.isGrounded && !jumping) {
            Vector3 jumpSpeed = playerCont.moveForce * (playerCont.walkSpeed - playerCont.airSpeed) + new Vector3(0, playerCont.jumpSpeed, 0);
            playerCont.JumpOnce(jumpSpeed, true);
            jumping = true;
            return tierCosts[0];
        } else {
            return 0;
        }
    }

    public override int TierTwo() {
        if (!playerCont.isGrounded && !doubleJumping && !wallJump) {
            playerCont.JumpOnce(dJumpSpeed, false);
            doubleJumping = true;
            return tierCosts[1];
        }
        return 0;
    }

    public override int TierThree() {
        // walljump (atm)
        if (!playerCont.isGrounded && !wallJumping && wallJump) {
            playerCont.JumpOnce(wJumpSpeed, false);
            wallJumping = true;
            return tierCosts[2];
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
        }
    }
}