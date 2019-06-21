using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public CanvasGroup panel;
    public bool isActive = false;
    public bool canControl = false;
    public GameObject firstButton;

    [Range(0, 1)]
    public float fadeSpeed;

    private float fade = 0;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        canControl = (fade == 0);
        panel.interactable = isActive;
        panel.alpha = fade;

        if (fade != 1 && isActive || fade != 0 && !isActive)
            fade = Mathf.MoveTowards(fade, isActive ? 1 : 0, fadeSpeed);

        Time.timeScale = canControl ? 1 : 0;
    }

    public void Button_Return() {
        isActive = false;
    }

    public void Button_Reset() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Button_Options

    public void Button_Exit() {
        SceneManager.LoadScene("Menu");
    }
}
