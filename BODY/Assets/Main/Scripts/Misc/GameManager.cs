using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Cinemachine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SerializedMonoBehaviour {
    public static GameManager instance;

    [SerializeField, TabGroup("References")]
    private PauseMenu pauseMenu;

    [SerializeField, TabGroup("References")]
    public Animator fadeAnim;

    [TabGroup("References"), Required]
    public RawImage camImage;

    [SerializeField, TabGroup("References"), Required]
    private AudioSource audioSource;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private PuzzleDoor elevatorDoor;

    private float audioMaxValue;

    public bool playerInHub = true;

    public bool CanControl { set {
            PlayerController cont = player.GetComponent<PlayerController>();
            var limbs = player.GetComponents<Limb>();
            foreach (Limb limb in limbs) {
                limb.canControl = value;
            }
            cont.enabled = value;
            cont.rigid.velocity = (value ? cont.rigid.velocity : Vector3.zero);
            cont.rigid.constraints = RigidbodyConstraints.FreezeRotation;
                } }
    
    [SerializeField, TabGroup("Debugging")]
    public PuzzleManager aktPuzzle;

    // Start is called before the first frame update
    void Start() {
        instance = this;

        var limbs = player.GetComponents<Limb>();
        foreach (Limb limb in limbs) {
            limb.canControl = true;
        }
        Cursor.visible = false;
        audioMaxValue = audioSource.volume;
        StartCoroutine(Intro());
    }

    private IEnumerator Intro() {
        ScreenShakeManager.Instance.Shake(0.1f, 2, 5);
        yield return new WaitForSeconds(5);
        elevatorDoor.gotActive = true;
    }

    // Update is called once per frame
    void Update() {
        InputHandler();
        EventHelper.FixEventSystem();
        //if (playerInHub) {
        //    audioSource.volume = audioMaxValue;
        //} else {
        //    audioSource.volume = audioMaxValue/3;
        //}
    }

    void InputHandler() {
        if (Input.GetButtonDown("ButtonStart")) {
            pauseMenu.SetPauseMenu();

            EventSystem es = EventSystem.current;
            es.SetSelectedGameObject(pauseMenu.firstButton);
        }

        if (Input.GetButtonDown("SelectButton") && aktPuzzle != null) {
            SceneManager.LoadScene(0);
            //aktPuzzle.ResetPuzzle(true);
            //aktPuzzle.StartCoroutine(aktPuzzle.ResetPuzzle(false));
        }
    }
}