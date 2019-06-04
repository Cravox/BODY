using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Head : Limb {
    [SerializeField]
    private MovingPlatform[] platforms = new MovingPlatform[2];

    public override int TierOne() {
        for (int i = 0; i < platforms.Length; i++) {
            platforms[i].platCol.enabled = !platforms[i].platCol.enabled;
        }
        return tierCosts[0];
    }

    public override int TierTwo() {
        MovingPlatform.stop = !MovingPlatform.stop;
        return tierCosts[1];
    }

    public override int TierThree() {
        MovingPlatform.dirChangeActive = !MovingPlatform.dirChangeActive;
        return tierCosts[2];
    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {

    }
}