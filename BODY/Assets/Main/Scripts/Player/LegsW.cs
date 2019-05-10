using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsW : MonoBehaviour, IPlayerLimb {
    public PlayerController playerCont { get { return PlayerController.instance; } }
    public bool FullyCharged { get { return (energyState == Enums.EnergyStates.FULLY_CHARGED); } }

    public Enums.EnergyStates EnergyState { get { return this.energyState; } set { energyState = value; } }
    private Enums.EnergyStates energyState = Enums.EnergyStates.ZERO_CHARGES;

    public Enums.Limb Limb { get { return limb; } }
    private Enums.Limb limb = Enums.Limb.LEGS;

    public bool isDashing;
    public float dashSpeed = 10;
    public float dashDuration = 1;
    public PlayerForce dashForce;


    public void Charge() {
        energyState++;
        print(energyState);
    }

    public void Discharge() {
        energyState = Enums.EnergyStates.ZERO_CHARGES;
    }

    public void TierOne() {
        if (Input.GetButtonDown("Jump") && !playerCont.isGrounded && !isDashing)
        {
            if (!isDashing)
            {
                isDashing = true;
                playerCont.modelAnim.Play("Dash");
                playerCont.AddForce(playerCont.modelAxis.forward * dashSpeed, dashDuration, out dashForce);
            }
        }

        if (Input.GetButtonUp("Jump") && isDashing && dashForce != null)
            dashForce.Stop();

        if (playerCont.isGrounded && isDashing && dashForce != null)
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
        switch (energyState) {
            case Enums.EnergyStates.ZERO_CHARGES:
                break;
            case Enums.EnergyStates.ONE_CHARGE:
                TierOne();
                break;
            case Enums.EnergyStates.TWO_CHARGES:
                TierOne();
                TierTwo();
                break;
            case Enums.EnergyStates.FULLY_CHARGED:
                break;
            default:
                break;
        }
    }
}