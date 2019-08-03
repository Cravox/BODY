using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCamera : MonoBehaviour {
    [SerializeField]
    private Transform playerTrans;

    // Start is called before the first frame update
    void Start() {
        playerTrans = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update() {
        if (playerTrans != null)
            transform.LookAt(playerTrans);
    }
}