using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class EnergySystem : SerializedMonoBehaviour {
    [SerializeField, TabGroup("Balancing")]
    private int energyPoints = 100;

    [Required, SerializeField, Tooltip("From Top to Bottom: Head, Arms, Legs"), TabGroup("References")]
    private Limb[] PlayerLimbs = new Limb[3];

    [SerializeField, TabGroup("References")]
    private Text energyText;

    [SerializeField, TabGroup("References")]
    private Image stateImage;

    private int maxEnergy;

    private float leftTriggerInput;
    private float rightTriggerInput;
    
    private string[] stateString = new string[3];

    // Start is called before the first frame update
    void Start() {
        maxEnergy = energyPoints;
        stateImage.enabled = true;
    }

    // Update is called once per frame
    void Update() {
        Enums.LimbIndex? myIndex = null;
        int eCost = 0;

        if (!GameManager.instance.inPauseMenu)
        {
            if (Input.GetButtonDown("ButtonY")) myIndex = Enums.LimbIndex.HEAD;
            if (Input.GetButtonDown("ButtonX")) myIndex = Enums.LimbIndex.ARMS;
            if (Input.GetButtonDown("Jump")) myIndex = Enums.LimbIndex.LEGS;
        }

        leftTriggerInput = Input.GetAxis("LeftTrigger");
        rightTriggerInput = Input.GetAxis("RightTrigger");

        if (rightTriggerInput >= 0.9f && leftTriggerInput >= 0.9f) {
            SetLimbState(Enums.ChargeState.TIER_TWO);
        } else if (leftTriggerInput >= 0.9f) {
            SetLimbState(Enums.ChargeState.TIER_ONE);
        } else {
            SetLimbState(Enums.ChargeState.NOT_CHARGED);
        }

        if (myIndex == null) return;

        var limb = PlayerLimbs[(int)myIndex];

        if(limb.chargeState == Enums.ChargeState.TIER_TWO) {
            eCost = limb.TierTwo();
        } else if (limb.chargeState == Enums.ChargeState.TIER_ONE) {
            eCost = limb.TierOne();
        } else {
            limb.BaselineAbility();
        }

        if(!GameManager.instance.playerInHub) energyPoints -= eCost;

        UpdateEnergyUI();
    }

    void SetLimbState(Enums.ChargeState cs) {
        foreach (Limb limb in PlayerLimbs) {
            limb.chargeState = cs;
        }
    }

    void UpdateEnergyUI() {
        energyText.text = "Energy State: " + energyPoints + "%";
    }
}