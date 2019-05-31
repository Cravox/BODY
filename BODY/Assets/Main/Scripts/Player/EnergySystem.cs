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

    public enum ChargeState : int {
        NOT_CHARGING,
        CHARGING,
        DISCHARGING
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

    [SerializeField, TabGroup("References")]
    private Image stateImage;

    [SerializeField, TabGroup("References")]
    private Text stateText;

    private int maxEnergy;

    private float leftTriggerInput;
    private float rightTriggerInput;

    private string[] stateString = new string[2];

    public static ChargeState chargeState;

    // Start is called before the first frame update
    void Start() {
        maxEnergy = energyPoints;
        stateString[0] = "CHARGING";
        stateString[1] = "DISCHARGING";
    }

    // Update is called once per frame
    void Update() {
        leftTriggerInput = Input.GetAxis("LeftTrigger");
        rightTriggerInput = Input.GetAxis("RightTrigger");

        LimbIndex? myIndex = null;

        if (Input.GetButtonDown("ButtonY")) myIndex = LimbIndex.HEAD;
        if (Input.GetButtonDown("ButtonX")) myIndex = LimbIndex.ARMS;
        if (Input.GetButtonDown("Jump")) myIndex = LimbIndex.LEGS;

        if (rightTriggerInput >= 0.8f) {
            ShowStateUI(true);
            chargeState = ChargeState.CHARGING;
        } else if (leftTriggerInput >= 0.8f) {
            ShowStateUI(false);
            chargeState = ChargeState.DISCHARGING;
        } else {
            stateImage.enabled = false;
            stateText.text = "";
            chargeState = ChargeState.NOT_CHARGING;
        }

        // charge Leg if player presses A and theres no energy in legs
        if (chargeState == ChargeState.NOT_CHARGING) {
            if (myIndex == LimbIndex.LEGS && PlayerLimbs[(int)myIndex].EnergyState == Enums.EnergyStates.ZERO_CHARGES) {
                ChargeLimb(PlayerLimbs[(int)myIndex], chargeAmount);
                UpdateEnergyUI();
            }
        }

        // checks if player wants to charge or discharge limb
        if (myIndex != null) {
            if (chargeState == ChargeState.CHARGING) {
                ChargeLimb(PlayerLimbs[(int)myIndex], chargeAmount);
            } else if (chargeState == ChargeState.DISCHARGING) {
                if (PlayerLimbs[(int)myIndex].EnergyState != Enums.EnergyStates.ZERO_CHARGES) {
                    DischargeLimb(PlayerLimbs[(int)myIndex], chargeAmount);
                }
            }
        }

        if (Input.GetButtonDown("LeftBumper")) {
            ResetEnergy();
        }

        if (Input.GetButtonDown("ButtonB")) {
            BalanceEnergy();
        }
    }

    void ShowStateUI(bool charge) {
        stateImage.enabled = true;

        if (charge) stateText.text = stateString[0];
        else stateText.text = stateString[1];
    }

    // put energy from one limb to another one
    void SwitchEnergy(LimbIndex from, LimbIndex to) {
        var fromLimb = PlayerLimbs[(int)from];
        var toLimb = PlayerLimbs[(int)to];
        if (fromLimb.EnergyState != Enums.EnergyStates.ZERO_CHARGES && toLimb.FullyCharged) {
            fromLimb.Discharge(chargeAmount);
            toLimb.Charge(chargeAmount);
        }
    }

    void ChargeLimb(Limb limb, int amount) {
        if (!limb.FullyCharged && energyPoints >= amount) {
            energyPoints -= amount;
            limb.Charge(amount);
        }
        UpdateEnergyUI();
    }

    void DischargeLimb(Limb limb, int amount) {
        if (limb.EnergyState != Enums.EnergyStates.ZERO_CHARGES) {
            energyPoints += amount;
            limb.Discharge(amount);
        }
        UpdateEnergyUI();
    }

    void UpdateEnergyUI() {
        energyText.text = "Energy State: " + ((energyPoints+1) * 10) + "%";
    }

    //reset all energy
    void ResetEnergy() {
        energyPoints = maxEnergy;
        foreach (Limb limb in PlayerLimbs) {
            limb.Discharge();
        }
        UpdateEnergyUI();
    }

    // resets energy and puts 1 energy-charge in each limb
    void BalanceEnergy() {
        ResetEnergy();
        foreach (Limb limb in PlayerLimbs) {
            ChargeLimb(limb, 3);
        }
        UpdateEnergyUI();
    }
}