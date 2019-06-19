using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : SerializedMonoBehaviour {
    [SerializeField, TabGroup("References")]
    private PauseMenu pauseMenu;

    [SerializeField, TabGroup("Debugging")]
    public static PuzzleManager aktPuzzle;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        InputHandler();
    }

    void InputHandler() {
        if (Input.GetButtonDown("ButtonPause")) {
            pauseMenu.isActive = !pauseMenu.isActive;

            EventSystem es = EventSystem.current;
            es.SetSelectedGameObject(pauseMenu.firstButton);
        }

        if (Input.GetButtonDown("SelectButton")) {
            aktPuzzle.ResetObjects();
        }
    }
}