using System.Collections;
using System.Collections.Generic;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine;

public class InfoScreen : SerializedMonoBehaviour {
    [SerializeField]
    private string screenText;

    [SerializeField, TabGroup("References"), Required]
    private TextMeshProUGUI gUI;

    [SerializeField, TabGroup("References"), Required]
    private Animator screenAnim;

    // Start is called before the first frame update
    void Start() {
        gUI.text = screenText;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            screenAnim.SetBool("active", true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            screenAnim.SetBool("active", false);
        }
    }
}
