using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Head : Limb {
    protected override string limbName => "Head";
    public GameObject light;
    public bool lightActive;

    public override void TierOne() {
        if (Input.GetButtonDown("ButtonY")) {
            lightActive = !lightActive;
            light.SetActive(lightActive);
        }
    }

    public override void TierTwo() {

    }

    public override void TierThree() {

    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {

    }
}