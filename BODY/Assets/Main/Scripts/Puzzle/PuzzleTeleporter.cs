using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PuzzleTeleporter : TriggerContainer {
    [SerializeField, TabGroup("References"), Required]
    private PuzzleManager pManager;

    [SerializeField, TabGroup("References"), Required]
    private ParticleSystem teleporterActiveVFX;

    //[SerializeField, TabGroup("References"), Required]
    //private ParticleSystem teleporterPortVFX;
    
    [SerializeField]
    private bool activated = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (gotActive && !activated) {
            teleporterActiveVFX.Play();
            activated = true;
        } else if (!gotActive) {
            teleporterActiveVFX.Stop();
            activated = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && gotActive) {
            pManager.StartCoroutine(pManager.ResetPuzzle(true));
        }
    }
}