using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

    [Header("Scene Setup")]
    private Scene stoLoad;
    public string sceneToLoad;

    [Header("Panel Setup")]
    public GameObject firstObject;
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("Settings Objects")]
    public GameObject settingsFirstObject;
    public Text volumeText;
    public Toggle AxisX;
    public Toggle AxisY;

    private int currentPanel;

    private int volume = 100;
    private bool invAxisX = false;
    private bool invAxisY = false;

    private GameObject currentActive;

    void Awake()
    {
        LoadSettings();
    }

    void Update()
    {
        InputController();
        EventHelper.FixEventSystem();
    }

    void InputController()
    {
        if(Input.GetButtonDown("ButtonB") && currentPanel != 0)
            CloseMenu();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("BODY_Volume"))
        {
            volume = PlayerPrefs.GetInt("BODY_Volume");
            invAxisX = PlayerPrefs.GetInt("BODY_XInverted") == 1 ? true : false;
            invAxisY = PlayerPrefs.GetInt("BODY_YInverted") == 1 ? true : false;
        }
        else
        {
            SaveSettings();
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("BODY_Volume", volume);
        PlayerPrefs.SetInt("BODY_XInverted", invAxisX ? 1 : 0);
        PlayerPrefs.SetInt("BODY_YInverted", invAxisY ? 1 : 0);
    }

    public void OpenMenu(int panel)
    {
        currentPanel = panel;

        switch (panel)
        {
            case 1: //settings
                LoadSettings();
                settingsPanel.SetActive(true);
                volumeText.text = volume.ToString() + "%";
                AxisX.isOn = invAxisX;
                AxisY.isOn = invAxisY;
                EventSystem.current.SetSelectedGameObject(settingsFirstObject);
                break;
            case 2: //credits
                creditsPanel.SetActive(true);
                EventSystem.current.SetSelectedGameObject(null);
                break;
        }
    }

    public void CloseMenu()
    {
        switch (currentPanel)
        {
            case 1: //settings
                invAxisX = AxisX.isOn;
                invAxisY = AxisY.isOn;
                SaveSettings();
                settingsPanel.SetActive(false);
                break;
            case 2: //credits
                creditsPanel.SetActive(false);
                break;
        }

        EventSystem.current.SetSelectedGameObject(firstObject);
        currentPanel = 0;
    }

    public void Button_Start() {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void Button_Options()
    {
        OpenMenu(1);
    }

    public void Button_Credits()
    {
        OpenMenu(2);
    }

    public void Button_ExitGame() {
        Application.Quit();
    }

    public void Button_AddRemoveVolume(int amount)
    {
        if (volume + amount > 100)
            volume = 100;
        else if (volume + amount < 0)
            volume = 0;
        else
            volume += amount;

        volumeText.text = volume.ToString() + "%";
    }
}