using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Head : Limb {
    protected override string limbName => "Head";

    [SerializeField]
    private MovingPlatform[] platforms = new MovingPlatform[2];

    public override void TierOne() {
        for (int i = 0; i < platforms.Length; i++) {
            platforms[i].platCol.enabled = true;
        }
    }

    public override void TierTwo() {
            MovingPlatform.stop = false;
    }

    public override void TierThree() {
        if(EnergySystem.chargeState == EnergySystem.ChargeState.NOT_CHARGING) {
            if (Input.GetButtonDown("ButtonY"))
                MovingPlatform.dirChangeActive = !MovingPlatform.dirChangeActive;
        }
    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {

    }

    protected override void OnDischarge() {
        switch (EnergyState) {
            case Enums.EnergyStates.ZERO_CHARGES:
                for (int i = 0; i < platforms.Length; i++) {
                    platforms[i].platCol.enabled = false;
                }
                MovingPlatform.dirChangeActive = false;
                MovingPlatform.stop = true;
                break;
            case Enums.EnergyStates.THREE_CHARGES:
                MovingPlatform.stop = true;
                break;
            case Enums.EnergyStates.SIX_CHARGES:
                break;
            case Enums.EnergyStates.FULLY_CHARGED:
                break;
            default:
                break;
        }
    }
}