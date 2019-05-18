using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour {
    [SerializeField]
    private Text energyPointsText;

    [SerializeField, Tooltip("From Top to Bottom: Head, Arms, Legs")]
    private Text[] limbText = new Text[3];

    [SerializeField]
    private Color[] imageColors = new Color[4];

    [SerializeField]
    private Image[] limbImage = new Image[3];

    private int[] limbPoints = new int[3];

    private string[] limbStrings = new string[3];

    // Start is called before the first frame update
    void Start() {
        limbStrings[0] = "Head State: ";
        limbStrings[1] = "Arms State: ";
        limbStrings[2] = "Legs State: ";
    }

    // Update is called once per frame
    void Update() {

    }

    public void Init(int energyPoints) {
        energyPointsText.text = "Energy State: " + ((energyPoints * 10) + 10) + "%";
    }

    public void UpdateText(Limb limb, int energyPoints, int index) {
        switch (limb.EnergyState) {
            case Enums.EnergyStates.ZERO_CHARGES:
                limbImage[limb.index].color = imageColors[0];
                break;
            case Enums.EnergyStates.ONE_CHARGE:
                break;
            case Enums.EnergyStates.TWO_CHARGES:
                break;
            case Enums.EnergyStates.THREE_CHARGES:
                limbImage[limb.index].color = imageColors[1];
                break;
            case Enums.EnergyStates.FOUR_CHARGES:
                break;
            case Enums.EnergyStates.FIVE_CHARGES:
                break;
            case Enums.EnergyStates.SIX_CHARGES:
                limbImage[limb.index].color = imageColors[2];
                break;
            case Enums.EnergyStates.SEVEN_CHARGES:
                break;
            case Enums.EnergyStates.EIGHT_CHARGES:
                break;
            case Enums.EnergyStates.FULLY_CHARGED:
                limbImage[limb.index].color = imageColors[3];
                break;
            default:
                break;
        }
        var eState = (int)limb.EnergyState;

        energyPointsText.text = "Energy State: " + ((energyPoints * 10) + 10) + "%";
        limbText[index].text = limbStrings[index] + eState * 10 + "%";
    }

    public void ResetText(int energyPoints) {
        energyPointsText.text = "Energy State: " + ((energyPoints * 10) + 10) + "%";
        for (int i = 0; i < limbText.Length; i++) {
            limbImage[i].color = imageColors[0];
            limbText[i].text = limbStrings[i] + 0 + "%";
        }
    }
}