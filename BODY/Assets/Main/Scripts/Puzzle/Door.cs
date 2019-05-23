using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    [SerializeField]
    private Transform openPos;

    [SerializeField]
    public Transform closedPos;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void Open(bool open) {
        var desPos = open ? openPos : closedPos;

        transform.position = desPos.position;
    }
}