using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TriggerPlate : MonoBehaviour {
    [SerializeField, TabGroup("References"), Required]
    private Animator triggerAnim;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider col) {
        triggerAnim.SetBool("Triggered", true);
    }

    private void OnTriggerExit(Collider col) {
        triggerAnim.SetBool("Triggered", false);
    }
}