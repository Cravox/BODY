using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryBox : MonoBehaviour {
    [SerializeField]
    private bool isTrigger;

    [HideInInspector]
    public bool FirstPickUp = false;
    private bool pickedUp = false;

    [SerializeField]
    private List<Door> triggeredObjects;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (FirstPickUp && isTrigger && !pickedUp) {
            foreach (var door in triggeredObjects) {
                door.Open(true);
                pickedUp = true;
            }
        }
    }
}