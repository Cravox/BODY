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
    private bool disCharged = false;

    private float leftTriggerInput;
    private float rightTriggerInput;

    private string[] stateString = new string[2];

    public static bool isCharging;
    public static bool isDischarging;

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

        if (chargeState == ChargeState.NOT_CHARGING) {
            if (myIndex == LimbIndex.LEGS && PlayerLimbs[(int)myIndex].EnergyState == Enums.EnergyStates.ZERO_CHARGES) {
                ChargeLimb(PlayerLimbs[(int)myIndex], chargeAmount);
                UpdateEnergyUI();
            }
        }

        if (chargeState == ChargeState.CHARGING) {
            print("Charging");
            if (myIndex != null) {
                ChargeLimb(PlayerLimbs[(int)myIndex], chargeAmount);
            }
            UpdateEnergyUI();
        } else if (chargeState == ChargeState.DISCHARGING) {
            print("Discharging");
            if (myIndex != null) {
                if (PlayerLimbs[(int)myIndex].EnergyState != Enums.EnergyStates.ZERO_CHARGES) {
                    PlayerLimbs[(int)myIndex].Discharge(chargeAmount);
                    energyPoints += chargeAmount;
                }
            }
            UpdateEnergyUI();
        }

        //if (DPadButtons.Up) myIndex = LimbIndex.HEAD;
        //if (DPadButtons.Left) myIndex = LimbIndex.ARMS;
        //if (DPadButtons.Down) myIndex = LimbIndex.LEGS;

        //if (Input.GetButton("RightBumper") && myIndex != null) {
        //    if (PlayerLimbs[(int)myIndex].EnergyState != Enums.EnergyStates.ZERO_CHARGES) {
        //        PlayerLimbs[(int)myIndex].Discharge(chargeAmount);
        //        energyPoints += chargeAmount;
        //    }
        //    disCharged = true;
        //    UpdateEnergyUI();
        //}

        //if (!disCharged) {
        //    if (PlayerLimbs[(int)LimbIndex.ARMS].IsInteracting && myIndex != null && energyPoints == 0) {
        //        if (energyPoints == 0) {
        //            if (myIndex == LimbIndex.HEAD) {
        //                SwitchEnergy(LimbIndex.LEGS, LimbIndex.HEAD);
        //            } else if (myIndex == LimbIndex.LEGS) {
        //                SwitchEnergy(LimbIndex.HEAD, LimbIndex.LEGS);
        //            }
        //        } else {
        //            ChargeLimb(PlayerLimbs[(int)myIndex], chargeAmount);
        //        }
        //    } else if (myIndex != null) {
        //        ChargeLimb(PlayerLimbs[(int)myIndex], chargeAmount);
        //    }
        //}

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

    void SwitchEnergy(LimbIndex from, LimbIndex to) {
        var fromLimb = PlayerLimbs[(int)from];
        var toLimb = PlayerLimbs[(int)to];
        if (fromLimb.EnergyState != Enums.EnergyStates.ZERO_CHARGES && toLimb.FullyCharged) {
            fromLimb.Discharge(chargeAmount);
            toLimb.Charge(chargeAmount);
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