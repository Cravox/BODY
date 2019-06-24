using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public CanvasGroup panel;
    public GameObject firstButton;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetPauseMenu() {
        panel.interactable = !panel.interactable;
        panel.alpha = panel.interactable ? 1 : 0;

        GameManager.instance.CanControl = !panel.interactable;
        Time.timeScale = panel.interactable ? 0 : 1;
    }
    
    public void Button_Return() {
        SetPauseMenu();
    }

    public void Button_Reset() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Button_Options

    public void Button_Exit() {
        SceneManager.LoadScene("Menu");
    }
}
