using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryBox : MonoBehaviour {
    [SerializeField]
    private bool isTrigger;

    public bool pickedUp;

    [SerializeField]
    private GameObject triggeredObject;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (pickedUp && isTrigger) {

        }
    }
}
