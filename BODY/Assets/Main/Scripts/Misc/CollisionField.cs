using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionField : MonoBehaviour {
    public Transform obj;

    private void OnTriggerEnter(Collider other) {
        if (!other.isTrigger)
            obj = other.transform;
    }

    private void OnTriggerExit(Collider other) {
        obj = null;
    }
}
