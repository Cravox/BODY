using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class EnergySystem : SerializedMonoBehaviour {
    [Required, SerializeField, Tooltip("From Top to Bottom: Head, Arms, Legs"), TabGroup("References")]
    private Limb[] playerLimbs = new Limb[3];

    [SerializeField, TabGroup("References"), Required]
    private Text energyText;

    [SerializeField, TabGroup("References"), Required]
    private Image stateImage;

    [SerializeField, TabGroup("References"), Required]
    private Animator playerAnim;

    private int maxEnergy;
    private int eCost = 0;

    private float leftTriggerInput;
    private float rightTriggerInput;

    // Start is called before the first frame update
    void Start() {
        stateImage.enabled = true;
    }

    // Update is called once per frame
    void Update() {
        Enums.LimbIndex? myIndex = null;

        if (Input.GetButtonDown("ButtonY")) myIndex = Enums.LimbIndex.HEAD;
        if (Input.GetButtonDown("ButtonX")) myIndex = Enums.LimbIndex.ARMS;
        if (Input.GetButtonDown("Jump")) myIndex = Enums.LimbIndex.LEGS;

        leftTriggerInput = Input.GetAxis("LeftTrigger");
        rightTriggerInput = Input.GetAxis("RightTrigger");

        if (rightTriggerInput >= 0.9f && leftTriggerInput >= 0.9f) {
            playerAnim.SetBool("isFullPower", true);
            SetLimbState(Enums.ChargeState.TIER_TWO);
        } else if (leftTriggerInput >= 0.9f) {
            playerAnim.SetBool("isFullPower", false);
            SetLimbState(Enums.ChargeState.TIER_ONE);
        } else {
            playerAnim.SetBool("isFullPower", false);
            SetLimbState(Enums.ChargeState.NOT_CHARGED);
        }

        if (myIndex == null) return;
        eCost = 0;

        var limb = playerLimbs[(int)myIndex];

        if (limb.chargeState == Enums.ChargeState.TIER_TWO) {
            eCost = limb.TierTwo();
        } else if (limb.chargeState == Enums.ChargeState.TIER_ONE) {
            eCost = limb.TierOne();
        } else {
            limb.BaselineAbility();
        }

        if (!GameManager.instance.playerInHub) {
            GameManager.instance.aktPuzzle.UsedEnergyPoints += eCost;
            UpdateEnergyUI();
        }
    }

    void SetLimbState(Enums.ChargeState cs) {
        foreach (Limb limb in playerLimbs) {
            limb.chargeState = cs;
        }
    }

    void UpdateEnergyUI() {
        energyText.text = "Batteries used: " + GameManager.instance.aktPuzzle.UsedEnergyPoints;
    }
}