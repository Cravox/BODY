using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsW : MonoBehaviour, IPlayerLimb {
    public PlayerController playerCont { get { return PlayerController.instance; } }
    public bool FullyCharged { get { return (energyState == EnergyStates.FULLY_CHARGED); } }

    public EnergyStates EnergyState { get { return this.energyState; } set { energyState = value; } }
    private EnergyStates energyState = EnergyStates.NOT_CHARGED;

    public bool isDashing;
    public float dashSpeed = 10;
    public float dashDuration = 1;
    public float dashTimer;
    public PlayerForce dashForce;

    public void Charge() {
        if (FullyCharged) {
            print("Legs fully charged");
            return;
        }
        energyState++;
        print(energyState);
    }

    public void Discharge() {
        energyState = EnergyStates.NOT_CHARGED;
        print("Discharge Arms");
    }

    public void TierOne() {
        if (Input.GetButtonDown("Jump") && !playerCont.isGrounded && !isDashing) {
            if (!isDashing)
            {
                isDashing = true;
                playerCont.modelAnim.Play("Dash");
                dashForce = playerCont.AddForceReturn(playerCont.modelAxis.forward * dashSpeed, dashDuration);
            }
        }

        if(Input.GetButtonUp("Jump") && isDashing)
            dashForce.Stop();

        if (playerCont.isGrounded && isDashing)
        {
            dashForce.Stop();
            isDashing = false;
        }

        playerCont.modelAnim.SetBool("IsDashing", isDashing);
    }

    public void TierThree() {
        throw new System.NotImplementedException();
    }

    public void TierTwo() {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(energyState != EnergyStates.NOT_CHARGED) TierOne();
    }

}
