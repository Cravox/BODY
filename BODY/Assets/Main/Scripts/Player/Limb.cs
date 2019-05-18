using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(PlayerController))]
public abstract class Limb : SerializedMonoBehaviour {
    protected PlayerController playerCont;
    public bool FullyCharged { get; set; }
    public Enums.EnergyStates EnergyState;
    public abstract int index { get; }
    
    protected EnergyUI eUI;

    protected void Start() {
        playerCont = GetComponent<PlayerController>();
        LimbStart();
    }

    public void Charge(int amount) {
        EnergyState += amount;
    }

    public void Discharge() {
        EnergyState = Enums.EnergyStates.ZERO_CHARGES;
    }

    protected void Update() {
        switch (EnergyState) {
            case Enums.EnergyStates.ZERO_CHARGES:
                break;
            case Enums.EnergyStates.THREE_CHARGES:
                TierOne();
                break;
            case Enums.EnergyStates.SIX_CHARGES:
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
    protected abstract void LimbStart();

    public abstract void TierOne();
    public abstract void TierTwo();
    public abstract void TierThree();
}
