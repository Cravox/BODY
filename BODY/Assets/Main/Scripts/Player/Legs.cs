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

    protected override string limbName => "Legs";

    public override void TierOne() {
        if(Input.GetButtonDown("Jump") && playerCont.isGrounded) {
            playerCont.Jump();
        }
    }

    public override void TierTwo() {
        if (Input.GetButtonDown("Jump") && !playerCont.isGrounded && !doubleJumping)
        {
            doubleJumping = true;
            playerCont.modelAnim.Play("Dash");

            playerCont.rigid.velocity = new Vector3(playerCont.rigid.velocity.x, 0, playerCont.rigid.velocity.z);

            Vector3 dirForceSide = playerCont.modelAxis.forward * dJumpSpeed.z + playerCont.transform.right * dJumpSpeed.x;
            Vector3 dirForceUp = playerCont.transform.up * dJumpSpeed.y;

            playerCont.AddForce(dirForceUp, 0, out dJumpForce, true);
            playerCont.AddForce(dirForceSide, jumpForwardTime, out dJumpForce, false);
        }

        if (playerCont.isGrounded && doubleJumping && dJumpForce != null)
        {
            dJumpForce.Stop();
            doubleJumping = false;
        }

        playerCont.modelAnim.SetBool("IsDashing", doubleJumping);
    }

    public override void TierThree() {
        // gliding or walljumping
    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {

    }

    protected override void OnDeactivation() {

    }
}