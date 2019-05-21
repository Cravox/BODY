using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPadButtons : MonoBehaviour {

    public static bool Up, Down, Right, Left;
    private float lastX, lastY;

    // Start is called before the first frame update
    void Start() {
        Up = Down = Left = Right = false;
        lastX = lastY = 0;
    }

    // Update is called once per frame
    void Update() {
        float lastDpadX = lastX;
        float lastDpadY = lastY;

        if (Input.GetAxis("DPadHorizontal") != 0) {
            float DPadX = Input.GetAxis("DPadHorizontal");

            Right = DPadX == 1 && lastDpadX != 1;
            Left = DPadX == -1 && lastDpadX != -1;
            lastX = DPadX;
        } else {
            lastX = 0;
        }

        if (Input.GetAxis("DPadVertical") != 0) {
            float DPadY = Input.GetAxis("DPadVertical");

            Up = DPadY == 1 && lastDpadY != 1;
            Down = DPadY == -1 && lastDpadY != -1;

            lastY = DPadY;
        } else {
            lastY = 0;
        }
    }
}
