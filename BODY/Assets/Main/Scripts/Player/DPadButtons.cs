using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPadButtons : MonoBehaviour
{
    static public bool dPadUp;
    static public bool dPadDown;
    static public bool dPadRight;
    static public bool dPadLeft;

    private readonly float F = 0.99f;

    private bool buttonPressed = false;

    private float lastX;
    private float lastY;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lastX = Input.GetAxis("ChargeHorizontal");
        if(lastX < 0) {
            dPadLeft = true;
        } else if (lastX > 0) {
            dPadRight = true;
        } else if (lastX == 0) {
            dPadLeft = false;
            dPadRight = false;
        }
    }
}
