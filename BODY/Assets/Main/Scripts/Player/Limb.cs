using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(PlayerController))]
public abstract class Limb : SerializedMonoBehaviour {
    protected PlayerController playerCont;
    public bool FullyCharged { get; set; }
    public Enums.EnergyStates EnergyState;

    protected void Start() {
        playerCont = GetComponent<PlayerController>();
    }

    public void Charge() {
        EnergyState++;
    }

    public void Discharge() {
        EnergyState = Enums.EnergyStates.ZERO_CHARGES;
    }

    protected void Update() {
        switch (EnergyState) {
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
                TierThree();
                break;
            default:
                break;
        }
        LimbUpdate();
    }

    protected abstract void LimbUpdate();

    public abstract void TierOne();
    public abstract void TierTwo();
    public abstract void TierThree();
}
