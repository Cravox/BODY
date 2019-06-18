using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    private Scene stoLoad;
    public string sceneToLoad;
    public void Button_Start() {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void Button_ExitGame() {
        Application.Quit();
    }
}