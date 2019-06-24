using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : SerializedMonoBehaviour {
    public static GameManager instance;

    public bool playerInHub = true;

    [SerializeField, TabGroup("References")]
    private PauseMenu pauseMenu;

    //public bool inPauseMenu { get { return !pauseMenu.canControl; } }

    public bool canControl;

    [SerializeField, TabGroup("Debugging")]
    public static PuzzleManager aktPuzzle;

    // Start is called before the first frame update
    void Start() {
        instance = this;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        InputHandler();
    }

    void InputHandler() {
        if (Input.GetButtonDown("ButtonStart")) {
            pauseMenu.SetPauseMenu();

            EventSystem es = EventSystem.current;
            es.SetSelectedGameObject(pauseMenu.firstButton);
        }

        if (Input.GetButtonDown("SelectButton")) {
            aktPuzzle.ResetObjects();
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {
            EventSystem es = EventSystem.current;
            es.SetSelectedGameObject(pauseMenu.firstButton);
        }
    }
}