using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class EnergySystem : SerializedMonoBehaviour {
    [SerializeField, TabGroup("Balancing")]
    private int energyPoints = 100;

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

    private delegate void TierMethod();

    private string[] stateString = new string[3];

    // Start is called before the first frame update
    void Start() {
        maxEnergy = energyPoints;
        stateString[0] = "TIER_ONE";
        stateString[1] = "TIER_TWO";
        stateString[2] = "TIER_THREE";
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

        if(rightTriggerInput >= 0.9f && leftTriggerInput >= 0.9f) {
            stateText.text = stateString[2];
        } else if (leftTriggerInput >= 0.9f) {
            stateText.text = stateString[1];
        } else {
            stateText.text = stateString[0];
        }

        if(myIndex != null) {
            if (rightTriggerInput >= 0.9f) {
                energyPoints -= PlayerLimbs[(int)myIndex].TierThree();
            } else if (leftTriggerInput >= 0.9f) {
                energyPoints -= PlayerLimbs[(int)myIndex].TierTwo();
            } else {
                energyPoints -= PlayerLimbs[(int)myIndex].TierOne();
            }
        }

        //if (myIndex == null) return;
        //var limb = PlayerLimbs[(int)myIndex];

        UpdateEnergyUI();
    }

    void UpdateEnergyUI() {
        energyText.text = "Energy State: " + energyPoints + "%";
    }
}