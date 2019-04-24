using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LegsH : SerializedMonoBehaviour {
    // The players attached CharacterController
    [Required, SerializeField, HideInPlayMode]
    private CharacterController characterController;

    // how high the player can jump
    [Range(5,15)]
    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump() {
        
    }
}
