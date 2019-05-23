using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisTrigger : MonoBehaviour {
    [SerializeField]
    private List<Door> doors;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag("Player")) {
            foreach (var door in doors) {
                door.Open(false);
            }
        }
    }
}