using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateHolder : MonoBehaviour {
    [SerializeField]
    private CarryBox box;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (box != null && box.gettingCarried) {
            box = null;
        }
    }

    private void OnTriggerStay(Collider other) {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Carry")) {
            box = other.gameObject.GetComponent<CarryBox>();

            if (!box.gettingCarried) {
                box.transform.parent = this.transform;
                box.transform.position = this.transform.position;
                box.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                box.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                box.transform.localRotation = Quaternion.identity;
            }
        } else if (other.gameObject.CompareTag("EnergyWall") && box != null) {
            box.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            box.transform.parent = null;
            box = null;
        }
    }
}