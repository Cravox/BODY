using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSwap : TriggerContainer {
    [SerializeField]
    private Transform playerWall;

    [SerializeField]
    private Transform cubeWall;

    private bool swapped = false;

    Vector3 playerWallPos;
    Vector3 cubeWallPos;


    // Start is called before the first frame update
    void Start() {
        playerWallPos = playerWall.position;
        cubeWallPos = cubeWall.position;
    }

    // Update is called once per frame
    void Update() {
        if (gotActive && !swapped) {
            playerWall.position = cubeWallPos;
            cubeWall.position = playerWallPos;
            swapped = true;
        }
    }
}
