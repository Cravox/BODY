﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BoxPlacer : SerializedMonoBehaviour {
    [SerializeField, Required]
    private Transform[] boxTrans = new Transform[4];
    private int currBoxCount = 0;

    public bool Completed;
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(currBoxCount >= boxTrans.Length) {
            Completed = true;
        }
    }

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag("Carry")) {
            col.gameObject.transform.position = boxTrans[currBoxCount].position;
            col.gameObject.transform.eulerAngles = Vector3.zero;
            col.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            currBoxCount++;
        }
    }
}