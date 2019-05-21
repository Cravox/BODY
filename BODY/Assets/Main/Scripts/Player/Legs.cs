using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : Limb {
    public bool isDashing;
    public float dashSpeed = 10;
    public float dashDuration = 1;
    public PlayerForce dashForce;

    protected override string limbName => "Legs";

    public override void TierOne() {
        if (Input.GetButtonDown("Jump") && !playerCont.isGrounded && !isDashing) {
            if (!isDashing) {
                isDashing = true;
                playerCont.modelAnim.Play("Dash");
                playerCont.AddForce(playerCont.modelAxis.forward * dashSpeed, dashDuration, out dashForce);
            }
        }

        if (Input.GetButtonUp("Jump") && isDashing && dashForce != null)
            dashForce.Stop();

        if (playerCont.isGrounded && isDashing && dashForce != null) {
            dashForce.Stop();
            isDashing = false;
        }

        playerCont.modelAnim.SetBool("IsDashing", isDashing);
    }

    public override void TierTwo() {

    }

    public override void TierThree() {

    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {

    }
}