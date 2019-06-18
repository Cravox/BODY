using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PuzzleManager : SerializedMonoBehaviour {
    [SerializeField, TabGroup("Balancing")]
    private Transform[] resettableObjects;

    private Vector3[] startObjectPosition;
    private Vector3[] startObjectEulerAngles;

    // Start is called before the first frame update
    void Start() {
        startObjectPosition = new Vector3[resettableObjects.Length];
        startObjectEulerAngles = new Vector3[resettableObjects.Length];
        
        for (int i = 0; i < resettableObjects.Length; i++) {
            startObjectPosition[i] = resettableObjects[i].position;
            startObjectEulerAngles[i] = resettableObjects[i].eulerAngles;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("SelectButton")) {
            ResetObjects();
        }
    }

    void ResetObjects() {
        for (int i = 0; i < resettableObjects.Length; i++) {
            resettableObjects[i].position = startObjectPosition[i];
            resettableObjects[i].eulerAngles = startObjectEulerAngles[i];
        }

    }
}