using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableUI : MonoBehaviour {
    [SerializeField]
    private Image xButtonImage;

    [SerializeField]
    private Text buttonText;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetImageActive(string interaction) {
        buttonText.enabled = true;
        xButtonImage.enabled = true;

        buttonText.text = interaction;
    }

    public void SetImageInactive() {
        buttonText.enabled = false;
        xButtonImage.enabled = false;
    }
}
