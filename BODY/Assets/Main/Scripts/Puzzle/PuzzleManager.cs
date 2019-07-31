using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PuzzleManager : SerializedMonoBehaviour {
    //[HideInInspector]
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
    private GameObject[] resettableObjects;

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

    [TabGroup("References"), Required]
    public Camera PuzzleCamera;

    private Vector3[] startObjectPosition;
    private Vector3[] startObjectEulerAngles;

    [HideInInspector]
    public bool puzzleCompleted;

    // Start is called before the first frame update
    void Start() {
        startObjectPosition = new Vector3[resettableObjects.Length];
        startObjectEulerAngles = new Vector3[resettableObjects.Length];

        //textGUI.text = puzzleTheme + " \n NO_DATA";

        if(resettableObjects != null) {
            for (int i = 0; i < resettableObjects.Length; i++) {
                startObjectPosition[i] = resettableObjects[i].transform.position;
                startObjectEulerAngles[i] = resettableObjects[i].transform.eulerAngles;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (puzzleCompleted) {
            puzzleCompleted = false;
        }
    }

    public IEnumerator ResetPuzzle(bool completed) {
        GameManager.instance.fadeAnim.SetTrigger("Fade");
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < resettableObjects.Length; i++) {
            if(resettableObjects[i].GetComponent<PushBox>() != null) {
                resettableObjects[i].GetComponent<PushBox>().ResetBox();
            }
            resettableObjects[i].transform.position = startObjectPosition[i];
            resettableObjects[i].transform.eulerAngles = startObjectEulerAngles[i];
        }

        if (completed) {
            player.transform.position = levelEntrancePosition.position;
            player.transform.rotation = levelEntrancePosition.rotation;
        } else {
            player.transform.position = playerReset.position;
            player.transform.rotation = playerReset.rotation;
        }
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.fadeAnim.SetTrigger("Fade");
        if (completed) {
            UpdateScreenUI();
            GameManager.instance.playerInHub = true;
        }
        UsedEnergyPoints = 0;
        player.GetComponent<EnergySystem>().UpdateEnergyUI();
    }

    public void UpdateScreenUI() {
        if(UsedEnergyPoints < bestPuzzleScore) {
            bestPuzzleScore = UsedEnergyPoints;
        }
        textGUI.text = puzzleTheme + "\n" + "Your best usage of batteries: " + UsedEnergyPoints + ".\n" + "Minimum of Batteries are: " + minEnergyPoints + ".\n";
    }
}