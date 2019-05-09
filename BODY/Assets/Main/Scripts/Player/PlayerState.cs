using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerState : SerializedMonoBehaviour {
    [TabGroup("Balancing")]
    public int EnergyPoints = 3;

    [Required, SerializeField, Tooltip("From Top to Bottom: Head, Arms, Legs"), TabGroup("References")]
    public IPlayerLimb[] PlayerLimbInterfaces = new IPlayerLimb[3];

    [Required, SerializeField, TabGroup("References")]
    private EnergyUI energyUI;

    private int maxEnergy;

    // Start is called before the first frame update
    void Start() {
        maxEnergy = EnergyPoints;
        energyUI.Init(EnergyPoints);
    }

    // Update is called once per frame
    void Update() {
        if (DPadButtons.Up) {
            ChargeLimb(PlayerLimbInterfaces[(int)Enums.Limb.HEAD]);
        }

        if (DPadButtons.Left) {
            ChargeLimb(PlayerLimbInterfaces[(int)Enums.Limb.ARMS]);
        }

        if (DPadButtons.Down) {
            ChargeLimb(PlayerLimbInterfaces[(int)Enums.Limb.LEGS]);
        }

        if (Input.GetButtonDown("ResetEnergy")) {
            ResetEnergy();
        }
    }

    void ChargeLimb(IPlayerLimb limb) {
        if (!limb.FullyCharged && EnergyPoints > 0) {
            EnergyPoints--;
            limb.Charge();
            UpdateFeedback(limb);
        }
    }

    void ResetEnergy() {
        EnergyPoints = maxEnergy;
        foreach (IPlayerLimb limb in PlayerLimbInterfaces) {
            limb.Discharge();
        }
        energyUI.ResetText(maxEnergy);
    }

    void UpdateFeedback(IPlayerLimb limb) {
        energyUI.UpdateText(limb, EnergyPoints);
    }
}