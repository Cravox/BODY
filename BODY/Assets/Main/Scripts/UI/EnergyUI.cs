using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour {
    [SerializeField]
    private Text energyPointsText;

    [SerializeField, Tooltip("From Top to Bottom: Head, Arms, Legs")]
    private Text[] limbText = new Text[3];

    private int[] limbPoints = new int[3];

    private string[] limbStrings = new string[3];

    // Start is called before the first frame update
    void Start() {
        energyPointsText.text = "Energy Points: " + 3;
        limbStrings[0] = "Head State: ";
        limbStrings[1] = "Arms State: ";
        limbStrings[2] = "Legs State: ";
    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateText(IPlayerLimb ILimb, int energyPoints) {
        var eState = (int)ILimb.EnergyState;
        var limb = (int)ILimb.Limb;

        energyPointsText.text = "Energy Points: " + energyPoints;
        limbText[limb].text = limbStrings[limb] + eState;
    }

    public void ResetText() {
        energyPointsText.text = "Energy Points: " + 3;
        for (int i = 0; i < limbText.Length; i++) {
            limbText[i].text = limbStrings[i] + 0;
        }
    }
}