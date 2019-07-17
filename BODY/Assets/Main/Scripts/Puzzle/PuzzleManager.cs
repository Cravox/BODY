﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PuzzleManager : SerializedMonoBehaviour {
    [HideInInspector]
    public int UsedEnergyPoints;

    [SerializeField, TabGroup("Balancing")]
    private string puzzleTheme;

    [SerializeField, TabGroup("Balancing")]
    private int minEnergyPoints;

    [SerializeField, TabGroup("Balancing")]
    private Transform[] resettableObjects;

    [SerializeField, TabGroup("References"), Required]
    private TextMeshProUGUI textGUI;
    
    private Vector3[] startObjectPosition;
    private Vector3[] startObjectEulerAngles;

    private GameObject puzzleDoor;

    // Start is called before the first frame update
    void Start() {
        startObjectPosition = new Vector3[resettableObjects.Length];
        startObjectEulerAngles = new Vector3[resettableObjects.Length];

        textGUI.text = puzzleTheme + "\n" + "You've used:" + UsedEnergyPoints + ".\n" + "Minimum of Energypoints are:" + minEnergyPoints + ".\n";

        for (int i = 0; i < resettableObjects.Length; i++) {
            startObjectPosition[i] = resettableObjects[i].position;
            startObjectEulerAngles[i] = resettableObjects[i].eulerAngles;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void ResetObjects() {
        for (int i = 0; i < resettableObjects.Length; i++) {
            resettableObjects[i].position = startObjectPosition[i];
            resettableObjects[i].eulerAngles = startObjectEulerAngles[i];
        }
    }

    public void UpdateScreenUI() {
        textGUI.text = puzzleTheme + "\n" + "You've used:" + UsedEnergyPoints + ".\n" + "Minimum of Energypoints are:" + minEnergyPoints + ".\n";
    }
}