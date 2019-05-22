﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableUI : MonoBehaviour {
    [SerializeField]
    private Image xButtonImage;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetImageActive(bool show) {
        xButtonImage.enabled = show;
    }
}