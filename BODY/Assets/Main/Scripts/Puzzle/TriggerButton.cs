using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButton : TriggerObject {
    public Animator buttonAnim;

    [SerializeField]
    private bool player;

    [SerializeField]
    private bool carryBox;

    [SerializeField]
    private bool pushBox;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (buttonAnim)
            buttonAnim.SetBool("Triggered", triggered);
    }

    private void OnTriggerStay(Collider other) {
        if (player) {
            if (other.gameObject.tag == "Player") {
                if(!triggered && !GetComponent<AudioSource>())
                    SoundController.Play(gameObject, SoundController.Sounds.BUTTON_CLICK, 128, 0.5f);
                triggered = true;
            }
        }

        if (carryBox) {
            if (other.gameObject.tag == "Carry") {
                if (!triggered && !GetComponent<AudioSource>())
                    SoundController.Play(gameObject, SoundController.Sounds.BUTTON_CLICK, 128, 0.5f);
                triggered = true;
            }
        }

        if (pushBox) {
            if (other.gameObject.tag == "Push") {
                if (!triggered && !GetComponent<AudioSource>())
                    SoundController.Play(gameObject, SoundController.Sounds.BUTTON_CLICK, 128, 0.5f);
                triggered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (triggered && !GetComponent<AudioSource>())
            SoundController.Play(gameObject, SoundController.Sounds.BUTTON_CLICK, 128, 0.5f);

        if (player) {
            if (other.gameObject.tag == "Player") {
                triggered = false;
            }
        }

        if (carryBox) {
            if (other.gameObject.tag == "Carry") {
                triggered = false;
            }
        }

        if (pushBox) {
            if (other.gameObject.tag == "Push") {
                triggered = false;
            }
        }
    }
}