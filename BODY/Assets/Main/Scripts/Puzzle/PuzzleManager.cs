using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PuzzleManager : SerializedMonoBehaviour {
    [HideInInspector]
    public GameObject player;

    [HideInInspector]
    public int UsedEnergyPoints = 0;

    [TabGroup("Balancing")]
    public int bestPuzzleScore = 0;

    [SerializeField, TabGroup("Balancing")]
    private string puzzleTheme;

    [SerializeField, TabGroup("Balancing")]
    private int minEnergyPoints;

    [SerializeField, TabGroup("Balancing")]
    private Transform[] resettableObjects;

    [SerializeField, TabGroup("References"), Required]
    private TextMeshProUGUI textGUI;

    [SerializeField, TabGroup("References"), Required]
    private Transform playerReset;

    [SerializeField, TabGroup("References"), Required]
    private PuzzleEntrance puzzleEntrance;

    [SerializeField, TabGroup("References"), Required]
    private PuzzleExit puzzleExit;

    [SerializeField, TabGroup("References"), Required]
    private Transform levelEntrancePosition;

    private Vector3[] startObjectPosition;
    private Vector3[] startObjectEulerAngles;

    [HideInInspector]
    public bool puzzleCompleted;

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
        if (puzzleCompleted) {
            puzzleCompleted = false;
        }
    }

    public void ResetPuzzle(bool withPlayer) {
        if (withPlayer) {
            player.transform.position = playerReset.position;
            player.transform.rotation = playerReset.rotation;
        }

        for (int i = 0; i < resettableObjects.Length; i++) {
            resettableObjects[i].position = startObjectPosition[i];
            resettableObjects[i].eulerAngles = startObjectEulerAngles[i];
        }
    }

    public void CompletePuzzle() {
        ResetPuzzle(false);
        player.transform.position = levelEntrancePosition.position;
        player.transform.rotation = levelEntrancePosition.rotation;
        UpdateScreenUI();
        UsedEnergyPoints = 0;
    }

    public void UpdateScreenUI() {
        if(UsedEnergyPoints < bestPuzzleScore) {
            bestPuzzleScore = UsedEnergyPoints;
        }
        textGUI.text = puzzleTheme + "\n" + "Your best usage of batteries: " + UsedEnergyPoints + ".\n" + "Minimum of Batteries are: " + minEnergyPoints + ".\n";
    }
}