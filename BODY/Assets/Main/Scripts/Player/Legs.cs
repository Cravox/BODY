using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Legs : Limb {
    [TabGroup("Balancing"), Header("Double Jump"), Tooltip("X = Sideways, Y = Up, Z = Forward")]
    public Vector3 dJumpSpeed;
    [TabGroup("Balancing")]
    public float jumpForwardTime = 0.5f;

    [TabGroup("Debugging")]
    public bool doubleJumping;
    [TabGroup("Debugging")]
    public PlayerForce dJumpForce;

    public override int TierOne() {
        if (playerCont.isGrounded) {
            playerCont.Jump();
            return tierCosts[0];
        } else {
            return 0;
        }
    }

    public override int TierTwo() {
        if (!playerCont.isGrounded && !doubleJumping) {
            doubleJumping = true;
            playerCont.modelAnim.Play("Dash");

            playerCont.rigid.velocity = new Vector3(playerCont.rigid.velocity.x, 0, playerCont.rigid.velocity.z);

            Vector3 dirForceSide = playerCont.modelAxis.forward * dJumpSpeed.z + playerCont.transform.right * dJumpSpeed.x;
            Vector3 dirForceUp = playerCont.transform.up * dJumpSpeed.y;

            playerCont.AddForce(dirForceUp, 0, out dJumpForce, true);
            playerCont.AddForce(dirForceSide, jumpForwardTime, out dJumpForce, false);
        }

        if (playerCont.isGrounded && doubleJumping && dJumpForce != null) {
            dJumpForce.Stop();
            doubleJumping = false;
            return tierCosts[1];
        }

        playerCont.modelAnim.SetBool("IsDashing", doubleJumping);
        return 0;
    }

    public override int TierThree() {
        // gliding or walljumping
        return tierCosts[2];
    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {

    }
}