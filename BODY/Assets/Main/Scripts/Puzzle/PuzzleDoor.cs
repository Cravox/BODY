using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PuzzleDoor : TriggerContainer {
    public bool isHubDoor;

    [SerializeField]
    private Color color;

    [SerializeField, TabGroup("References"), Required]
    private Animator doorAnim;
    private bool wasActive;

    [SerializeField]
    private MeshRenderer[] doorRenderer;

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

        //if(!isHubDoor)
        //    doorAnim.SetBool("Open", gotActive);
    }

    private void OnTriggerStay(Collider col) {
        if (col.gameObject.CompareTag("Player")) doorAnim.SetBool("Open", gotActive);
    }
}