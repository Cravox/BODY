using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Head : Limb {
    protected override string limbName => "Head";
    public GameObject headLight;

    public override void TierOne() {
        headLight.SetActive(true);
    }

    public override void TierTwo() {

    }

    public override void TierThree() {

    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {

    }

    protected override void OnDeactivation() {
        headLight.SetActive(false);
    }
}