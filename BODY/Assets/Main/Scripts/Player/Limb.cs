using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

// head, arms and legs inherit from limb
[RequireComponent(typeof(PlayerController))]
public abstract class Limb : SerializedMonoBehaviour {
    protected PlayerController playerCont;
    public bool FullyCharged { get; set; }
    public Enums.EnergyStates EnergyState;
    protected abstract string limbName { get; }
    public bool IsInteracting;

    // references the limbs attached UI-Image
    [SerializeField, TabGroup("References")]
    protected Image limbImage;

    // references the limbs attached UI-Text
    [SerializeField, TabGroup("References")]
    protected Text limbText;

    protected void Start() {
        playerCont = GetComponent<PlayerController>();
        LimbStart();
    }

    public void Charge(int amount) {
        EnergyState += amount;
        UpdateLimbUI();
    }

    public void Discharge() {
        EnergyState = Enums.EnergyStates.ZERO_CHARGES;
        UpdateLimbUI();
        OnDischarge();
    }

    public void Discharge(int amount) {
        if (EnergyState != Enums.EnergyStates.ZERO_CHARGES) {
            EnergyState -= amount;
            OnDischarge();
            UpdateLimbUI();
        }
    }

    public void UpdateLimbUI() {
        limbText.text = limbName + " State: " + ((int)EnergyState * 10) + "%";
        switch (EnergyState) {
            case Enums.EnergyStates.ZERO_CHARGES:
                limbImage.color = Color.white;
                break;
            case Enums.EnergyStates.THREE_CHARGES:
                limbImage.color = Color.red;
                break;
            case Enums.EnergyStates.SIX_CHARGES:
                limbImage.color = Color.yellow;
                break;
            case Enums.EnergyStates.FULLY_CHARGED:
                limbImage.color = Color.green;
                break;
            default:
                break;
        }
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
                TierOne();
                TierTwo();
                TierThree();
                break;
            default:
                break;
        }
        LimbUpdate();
    }

    public abstract void TierOne();
    public abstract void TierTwo();
    public abstract void TierThree();

    protected abstract void LimbUpdate();
    protected abstract void LimbStart();
    protected abstract void OnDischarge();
}
