using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerState : SerializedMonoBehaviour {
    [Required, SerializeField, Tooltip("From Top to Bottom: Head, Arms, Legs")]
    public IPlayerLimb[] PlayerLimbs = new IPlayerLimb[3];

    [Required, SerializeField, TabGroup("References")]
    private EnergyUI energyUI;

    public int EnergyPoints = 3;
    
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (DPadButtons.Up) {
            if(!PlayerLimbs[(int)Enums.Limb.HEAD].FullyCharged && EnergyPoints > 0) {
                EnergyPoints--;
                PlayerLimbs[(int)Enums.Limb.HEAD].Charge();
                UpdateFeedback(PlayerLimbs[(int)Enums.Limb.HEAD]);
            }
        }

        if (DPadButtons.Left) {
            if (!PlayerLimbs[(int)Enums.Limb.ARMS].FullyCharged && EnergyPoints > 0) {
                EnergyPoints--;
                PlayerLimbs[(int)Enums.Limb.ARMS].Charge();
                UpdateFeedback(PlayerLimbs[(int)Enums.Limb.ARMS]);
            }
        }

        if (DPadButtons.Down) {
            if (!PlayerLimbs[(int)Enums.Limb.LEGS].FullyCharged && EnergyPoints > 0) {
                EnergyPoints--;
                PlayerLimbs[(int)Enums.Limb.LEGS].Charge();
                UpdateFeedback(PlayerLimbs[(int)Enums.Limb.LEGS]);
            }
        }

        if (Input.GetButtonDown("ResetEnergy")) {
            EnergyPoints = 3;
            foreach (IPlayerLimb limb in PlayerLimbs) {
                limb.Discharge();
            }
            energyUI.ResetText();
        }
    }

    void UpdateFeedback(IPlayerLimb limb) {
        energyUI.UpdateText(limb, EnergyPoints);
    }
}
