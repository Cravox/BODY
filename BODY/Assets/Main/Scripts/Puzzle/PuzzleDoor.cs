using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PuzzleDoor : TriggerContainer {
    public bool isHubDoor;

    [SerializeField, TabGroup("References"), Required]
    private Animator doorAnim;
    private bool wasActive;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(!isHubDoor)
            doorAnim.SetBool("Open", gotActive);

        if(gotActive && !wasActive)
        {
            SoundController.Play(gameObject, SoundController.Sounds.DOOR_OPENCLOSE, 128, 0.5f);
            wasActive = true;
        }

        if (!gotActive && wasActive)
        {
            SoundController.Play(gameObject, SoundController.Sounds.DOOR_OPENCLOSE, 128, 0.5f);
            wasActive = false;
        }
    }

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.CompareTag("Player") && isHubDoor) doorAnim.SetBool("Open", true);
    }

    private void OnTriggerExit(Collider col) {
        if (col.gameObject.CompareTag("Player") && isHubDoor) doorAnim.SetBool("Open", false);
    }
}