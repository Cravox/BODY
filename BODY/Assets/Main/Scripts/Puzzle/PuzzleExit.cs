using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PuzzleExit : SerializedMonoBehaviour {
    [SerializeField, TabGroup("References"), Required]
    private PuzzleManager pManager;

    // Start is called before the first frame update
    void Start() {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GameManager.instance.playerInHub = true;
            GameManager.instance.aktPuzzle = null;
            pManager.PuzzleCamera.enabled = false;
            pManager.player = null;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            gameObject.SetActive(false);
        }
    }
}
