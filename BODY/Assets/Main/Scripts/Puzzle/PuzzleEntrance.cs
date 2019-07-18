using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PuzzleEntrance : SerializedMonoBehaviour {
    [SerializeField, TabGroup("References"), Required]
    private PuzzleManager pManager;

    [SerializeField, TabGroup("References"), Required]
    private PuzzleExit pExit;

    private bool firstEntered = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (!firstEntered) {
                firstEntered = true;
            }
            pManager.player = other.gameObject;
            GameManager.instance.aktPuzzle = pManager;
            GameManager.instance.playerInHub = false;
            pExit.gameObject.SetActive(true);
        }
    }
}