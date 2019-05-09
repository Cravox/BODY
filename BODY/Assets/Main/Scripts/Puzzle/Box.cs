using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Box : SerializedMonoBehaviour {
    [Required, SerializeField]
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update() {

    }
}