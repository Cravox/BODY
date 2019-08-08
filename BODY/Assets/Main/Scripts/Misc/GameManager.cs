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

    IEnumerator Intro() {
        ScreenShakeManager.Instance.Shake(0.1f, 2, SoundController.GetClip(SoundController.Voice.INTRO_1).length+SoundController.GetClip(SoundController.Voice.INTRO_2).length+SoundController.GetClip(SoundController.Voice.INTRO_3).length + 0.5f);
        SoundController.Play(this.gameObject, SoundController.Voice.INTRO_1);
        yield return new WaitForSeconds(SoundController.GetClip(SoundController.Voice.INTRO_1).length);
        SoundController.Play(this.gameObject, SoundController.Voice.INTRO_2);
        yield return new WaitForSeconds(SoundController.GetClip(SoundController.Voice.INTRO_2).length);
        SoundController.Play(this.gameObject, SoundController.Voice.INTRO_3);
        yield return new WaitForSeconds(SoundController.GetClip(SoundController.Voice.INTRO_3).length + 0.5f);
        elevatorDoor.gotActive = true;
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

        if (Input.GetButtonDown("SelectButton") && aktPuzzle != null) {
            SceneManager.LoadScene(0);
            //aktPuzzle.ResetPuzzle(true);
            //aktPuzzle.StartCoroutine(aktPuzzle.ResetPuzzle(false));
        }
    }
}