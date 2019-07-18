using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PuzzleTeleporter : MonoBehaviour {
    [SerializeField, TabGroup("References")]
    private PuzzleManager pManager;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            pManager.CompletePuzzle();
        }
    }
}