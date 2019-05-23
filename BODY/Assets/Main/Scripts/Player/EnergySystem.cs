﻿using System.Collections;
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

    [SerializeField, TabGroup("Balancing")]
    private int energyPoints = 9;

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
        maxEnergy = energyPoints;
    }

    // Update is called once per frame
    void Update() {
        LimbIndex? myIndex = null;

        if (DPadButtons.Up) myIndex = LimbIndex.HEAD;
        if (DPadButtons.Left) myIndex = LimbIndex.ARMS;
        if (DPadButtons.Down) myIndex = LimbIndex.LEGS;

        if (PlayerLimbs[(int)LimbIndex.ARMS].IsInteracting && myIndex != null && energyPoints == 0) {
            if(energyPoints == 0) {
                if (myIndex == LimbIndex.HEAD) {
                    SwitchEnergy(LimbIndex.LEGS, LimbIndex.HEAD);
                } else if (myIndex == LimbIndex.LEGS) {
                    SwitchEnergy(LimbIndex.HEAD, LimbIndex.LEGS);
                }
            } else {
                ChargeLimb(PlayerLimbs[(int)myIndex], chargeAmount);
            }
        } else if (myIndex != null) {
            ChargeLimb(PlayerLimbs[(int)myIndex], chargeAmount);
        }

        if (Input.GetButtonDown("LeftBumper")) {
            ResetEnergy();
        }

        if (Input.GetButtonDown("ButtonB")) {
            BalanceEnergy();
        }
    }

    void SwitchEnergy(LimbIndex from, LimbIndex to) {
        if(PlayerLimbs[(int)from].EnergyState != Enums.EnergyStates.ZERO_CHARGES && !PlayerLimbs[(int)to].FullyCharged) {
            PlayerLimbs[(int)from].Discharge(chargeAmount);
            PlayerLimbs[(int)to].Charge(chargeAmount);
        }
    }

    void ChargeLimb(Limb limb, int amount) {
        if (!limb.FullyCharged && energyPoints > 0) {
            energyPoints -= amount;
            limb.Charge(amount);
        }
        UpdateEnergyUI();
    }

    void UpdateEnergyUI() {
        energyText.text = "Energy State: " + ((energyPoints+1) * 10) + "%";
    }

    void ResetEnergy() {
        energyPoints = maxEnergy;
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