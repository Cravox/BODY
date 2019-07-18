using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PuzzleManager : SerializedMonoBehaviour {
    [HideInInspector]
    public int UsedEnergyPoints = 0;

    [HideInInspector]
    public int BestPuzzleHighScore = 0;

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

    // Start is called before the first frame update
    void Start() {
        startObjectPosition = new Vector3[resettableObjects.Length];
        startObjectEulerAngles = new Vector3[resettableObjects.Length];

        textGUI.text = puzzleTheme + " \n NO_DATA";

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
        textGUI.text = puzzleTheme + "\n" + "Your best usage of batteries: " + BestPuzzleHighScore + ".\n" + "Minimum of Batteries are: " + minEnergyPoints + ".\n";
    }
}