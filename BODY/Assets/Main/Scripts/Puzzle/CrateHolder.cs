using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateHolder : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Carry")) {
            if (!other.gameObject.GetComponent<CarryBox>().gettingCarried) {
                other.transform.parent = this.transform;
                other.transform.position = this.transform.position;
                other.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                other.transform.localRotation = Quaternion.identity;
            }
        }
    }
}