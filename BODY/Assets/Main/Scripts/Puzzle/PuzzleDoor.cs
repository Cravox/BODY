using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PuzzleDoor : MonoBehaviour {
    [SerializeField, TabGroup("References"), Required]
    private Animator doorAnim;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.CompareTag("Player")) doorAnim.SetBool("Open", true);
    }

    private void OnTriggerExit(Collider col) {
        if (col.gameObject.CompareTag("Player")) doorAnim.SetBool("Open", false);
    }
}