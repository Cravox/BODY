using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : SerializedMonoBehaviour {
    public static GameManager instance;

    [SerializeField, TabGroup("References")]
    private PauseMenu pauseMenu;

    [SerializeField]
    private GameObject player;

    public bool playerInHub = true;

    public bool CanControl { set {
            PlayerController cont = player.GetComponent<PlayerController>();
            player.GetComponent<EnergySystem>().enabled = value;
            cont.enabled = value;
            cont.rigid.velocity = (value ? cont.rigid.velocity : Vector3.zero); 
                } }
    
    [SerializeField, TabGroup("Debugging")]
    public PuzzleManager aktPuzzle;

    // Start is called before the first frame update
    void Start() {
        instance = this;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() {
        InputHandler();
        EventHelper.FixEventSystem();
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
    }
}