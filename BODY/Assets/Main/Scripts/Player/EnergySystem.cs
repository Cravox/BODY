using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class EnergySystem : SerializedMonoBehaviour {
    private enum LimbIndex : int {
        HEAD,
        ARMS,
        LEGS
    }

    [TabGroup("Balancing")]
    public int EnergyPoints = 3;

    [Required, SerializeField, Tooltip("From Top to Bottom: Head, Arms, Legs"), TabGroup("References")]
    public Limb[] PlayerLimbs = new Limb[3];

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
            ChargeLimb(PlayerLimbs[(int)LimbIndex.HEAD]);
        }

        if (DPadButtons.Left) {
            ChargeLimb(PlayerLimbs[(int)LimbIndex.ARMS]);
        }

        if (DPadButtons.Down) {
            ChargeLimb(PlayerLimbs[(int)LimbIndex.LEGS]);
        }

        if (Input.GetButtonDown("ResetEnergy")) {
            ResetEnergy();
        }
    }

    void ChargeLimb(Limb limb) {
        if (!limb.FullyCharged && EnergyPoints > 0) {
            EnergyPoints--;
            limb.Charge();
            UpdateFeedback(limb);
        }
    }

    void ResetEnergy() {
        EnergyPoints = maxEnergy;
        foreach (Limb limb in PlayerLimbs) {
            limb.Discharge();
        }
        energyUI.ResetText(maxEnergy);
    }

    void UpdateFeedback(Limb limb) {
        energyUI.UpdateText(limb, EnergyPoints);
    }
}