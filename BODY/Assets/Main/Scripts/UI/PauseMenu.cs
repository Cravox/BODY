using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public CanvasGroup panel;
    public bool isActive = false;
    public GameObject firstButton;

    [Range(0, 1)]
    public float fadeSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        panel.interactable = isActive;

        if (panel.alpha != 1 && isActive ||panel.alpha != 0 && !isActive)
            panel.alpha = Mathf.MoveTowards(panel.alpha, isActive ? 1 : 0, fadeSpeed);

        Time.timeScale = isActive ? 0 : 1;
    }

    public void Button_Return()
    {
        isActive = false;
    }
    public void Button_Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Button_Options

    public void Button_Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
