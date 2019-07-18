using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ZoneBox : SerializedMonoBehaviour {
    [SerializeField, TabGroup("References"), Required]
    private PuzzleManager pManager;

    private bool firstEntrance = false;

    private bool isPuzzleEntrance;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (isPuzzleEntrance) {
                GameManager.instance.playerInHub = false;
                GameManager.instance.aktPuzzle = pManager;
            } 
            else {
                if (!firstEntrance) {
                    pManager.BestPuzzleHighScore = pManager.UsedEnergyPoints;
                    pManager.UpdateScreenUI();
                    firstEntrance = true;
                } else if (pManager.UsedEnergyPoints < pManager.BestPuzzleHighScore) {
                    pManager.UpdateScreenUI();
                    pManager.UsedEnergyPoints = 0;
                    GameManager.instance.playerInHub = true;
                    GameManager.instance.aktPuzzle = null;
                }
            }
        }
    }
}