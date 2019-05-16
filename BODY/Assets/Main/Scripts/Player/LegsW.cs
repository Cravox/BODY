using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsW : Limb {//MonoBehaviour, IPlayerLimb {
    //public PlayerController playerCont { get { return PlayerController.instance; } }
    //public bool FullyCharged { get { return (energyState == Enums.EnergyStates.FULLY_CHARGED); } }
    //
    //public Enums.EnergyStates EnergyState { get { return this.energyState; } set { energyState = value; } }
    //private Enums.EnergyStates energyState = Enums.EnergyStates.ZERO_CHARGES;

    //public Enums.Limb Limb { get { return limb; } }
    //private Enums.Limb limb = Enums.Limb.LEGS;

    public bool isDashing;
    public float dashSpeed = 10;
    public float dashDuration = 1;
    public PlayerForce dashForce;

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

    public override void TierThree() {

    }

    public override void TierTwo() {

    }

    protected override void LimbUpdate() {

    }

    // Start is called before the first frame update
    void Start() {

    }
}