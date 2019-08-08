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

    [SerializeField]
    private MeshRenderer[] lineRenderers;

    [SerializeField]
    private Material[] lineMaterials;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (buttonAnim)
            buttonAnim.SetBool("Triggered", triggered);
    }

    private void OnTriggerEnter(Collider other) {
        if (player) {
            if (other.gameObject.tag == "Player") {
                foreach (var ren in lineRenderers) {
                    ren.material = lineMaterials[1];
                }
            }
        }

        if (carryBox) {
            if (other.gameObject.tag == "Carry") {
                foreach (var ren in lineRenderers) {
                    ren.material = lineMaterials[1];
                }
            }
        }

        if (pushBox) {
            if (other.gameObject.tag == "Push") {
                foreach (var ren in lineRenderers) {
                    ren.material = lineMaterials[1];
                }
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (player) {
            if (other.gameObject.tag == "Player") {
                triggered = true;
            }
        }

        if (carryBox) {
            if (other.gameObject.tag == "Carry") {
                triggered = true;
            }
        }

        if (pushBox) {
            if (other.gameObject.tag == "Push") {
                triggered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (triggered && !GetComponent<AudioSource>())
            SoundController.Play(gameObject, SoundController.Sounds.BUTTON_CLICK, 128, 0.25f);

        if (player) {
            if (other.gameObject.tag == "Player") {
                triggered = false;
                foreach (var ren in lineRenderers) {
                    ren.material = lineMaterials[1];
                }
            }
        }

        if (carryBox) {
            if (other.gameObject.tag == "Carry") {
                triggered = false;
                foreach (var ren in lineRenderers) {
                    ren.material = lineMaterials[0];
                }
            }
        }

        if (pushBox) {
            if (other.gameObject.tag == "Push") {
                triggered = false;
                foreach (var ren in lineRenderers) {
                    ren.material = lineMaterials[0];
                }
            }
        }
    }
}