using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerState : SerializedMonoBehaviour {
    [Required, SerializeField, Tooltip("From Top to Bottom: Head, Arms, Legs")]
    IPlayerLimb[] playerLimbs = new IPlayerLimb[3];

    private enum Limb : int {
        HEAD,
        ARMS,
        LEGS
    }

    private IPlayerLimb head;
    private IPlayerLimb arms;
    private IPlayerLimb legs;

    // Start is called before the first frame update
    void Start() {
        head = playerLimbs[0];
        arms = playerLimbs[1];
        legs = playerLimbs[2];
    }

    // Update is called once per frame
    void Update() {
        if (DPadButtons.Up) {
            playerLimbs[(int)Limb.HEAD].Charge();
            //head.Charge();
        }
        if (DPadButtons.Left) {
            playerLimbs[(int)Limb.ARMS].Charge();
            //arms.Charge();
        }
        if (DPadButtons.Down) {
            playerLimbs[(int)Limb.LEGS].Charge();
            //legs.Charge();
        }

        if (Input.GetButtonDown("ResetEnergy")) {
            foreach (IPlayerLimb limb in playerLimbs) {
                limb.Discharge();
            }
        }
    }

    void FixedUpdate() {

    }
}
