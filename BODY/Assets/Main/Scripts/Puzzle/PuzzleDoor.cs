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

    [SerializeField]
    private MeshRenderer[] doorRenderer;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (gotActive) {
            for (int i = 0; i < doorRenderer.Length; i++) {
                doorRenderer[i].material.SetColor("_EmissionColor", color);
            }
        } else {
            for (int i = 0; i < doorRenderer.Length; i++) {
                doorRenderer[i].material.SetColor("_EmissionColor", Color.white);
            }
        }

        //if(!isHubDoor)
        //    doorAnim.SetBool("Open", gotActive);
    }

    private void OnTriggerStay(Collider col) {
        if (col.gameObject.CompareTag("Player")) doorAnim.SetBool("Open", gotActive);
    }
}