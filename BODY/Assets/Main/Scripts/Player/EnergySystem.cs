using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class EnergySystem : SerializedMonoBehaviour {
    private enum LimbIndex : int {
        HEAD,
        ARMS,
        LEGS
    }

    [TabGroup("Balancing")]
    public int EnergyPoints = 6;

    [SerializeField, TabGroup("Balancing"), ValueDropdown("energyValue")]
    private int chargeAmount = 1;

    private int[] energyValue = new int[] {
        1,
        3
    };

    [Required, SerializeField, Tooltip("From Top to Bottom: Head, Arms, Legs"), TabGroup("References")]
    public Limb[] PlayerLimbs = new Limb[3];

    [SerializeField, TabGroup("References")]
    private Text energyText;

    private int maxEnergy;

    // Start is called before the first frame update
    void Start() {
        maxEnergy = EnergyPoints;

    }

    // Update is called once per frame
    void Update() {
        if (DPadButtons.Up) {
            ChargeLimb(PlayerLimbs[(int)LimbIndex.HEAD], chargeAmount);
        }

        if (DPadButtons.Left) {
            ChargeLimb(PlayerLimbs[(int)LimbIndex.ARMS], chargeAmount);
        }

        if (DPadButtons.Down) {
            ChargeLimb(PlayerLimbs[(int)LimbIndex.LEGS], chargeAmount);
        }

        if (Input.GetButtonDown("LeftBumper")) {
            ResetEnergy();
        }

        if (Input.GetButtonDown("ButtonB")) {
            BalanceEnergy();
        }
    }

    void ChargeLimb(Limb limb, int amount) {
        if (!limb.FullyCharged && EnergyPoints > 0) {
            EnergyPoints -= amount;
            limb.Charge(amount);
        }
        UpdateEnergyUI();
    }

    void UpdateEnergyUI() {
        energyText.text = "Energy State: " + ((EnergyPoints+1) * 10) + "%";
    }

    void ResetEnergy() {
        EnergyPoints = maxEnergy;
        foreach (Limb limb in PlayerLimbs) {
            limb.Discharge();
        }
        UpdateEnergyUI();
    }

    void BalanceEnergy() {
        ResetEnergy();
        foreach (Limb limb in PlayerLimbs) {
            ChargeLimb(limb, 3);
        }
        UpdateEnergyUI();
    }
}