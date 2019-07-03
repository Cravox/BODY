using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneBox : MonoBehaviour {
    public bool isPuzzleEntrance;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (isPuzzleEntrance)
                GameManager.instance.playerInHub = false;
            else
                GameManager.instance.playerInHub = true;
        }
    }
}