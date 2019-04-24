using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerH : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var moveX = Input.GetAxis("HorizontalRight");
        var moveY = Input.GetAxis("HorizontalLeft");
    }
}
