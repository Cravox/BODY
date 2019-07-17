using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Head : Limb {
    [SerializeField, TabGroup("Balancing")]
    private float maxDistancePlatform;

    [SerializeField, TabGroup("Balancing")]
    private float maxDistanceStasis;

    [SerializeField, TabGroup("Balancing")]
    private float maxStasisTime;

    [SerializeField, TabGroup("References"), Required]
    private Animator anim1;

    [SerializeField, TabGroup("References"), Required]
    private Animator anim2;

    [SerializeField, TabGroup("Debugging")]
    private List<MovingPlatform> activatedPlatforms = new List<MovingPlatform>();

    public List<MovingPlatform> pInDistance;

    public override void BaselineAbility() {
        //U BLIND BUT U SEE
    }

    public override int TierOne() {
        anim1.Play("anim", 0, 0);

        foreach (MovingPlatform platform in pInDistance) {
            platform.platCol.isTrigger = false;
            activatedPlatforms.Add(platform);
        }

        if (pInDistance.Count > 0) {
            return tierCosts[0];
        }

        return 0;
    }

    public override int TierTwo() {
        foreach (MovingPlatform t in activatedPlatforms) {
            t.stop = false;
        }

        if (activatedPlatforms.Count > 0) {
            activatedPlatforms.Clear();
            return tierCosts[1];
        }

        return 0;
    }

    public override int TierThree() {
        //List<StasisController> sInDistance = GetStasisObjects();

        //anim2.Play("anim", 0, 0);

        //foreach (StasisController sc in sInDistance) {
        //    sc.StartCoroutine(sc.Stasis(maxStasisTime));
        //}

        //if (sInDistance.Count > 0) {
        //    return tierCosts[2];
        //}

        //playerCont.modelAnim.Play("Dance");

        return 0;
    }

    private List<MovingPlatform> GetPlatformColliders() {
        List<MovingPlatform> t = new List<MovingPlatform>();

        Collider[] raySphere = Physics.OverlapSphere(transform.position, maxDistancePlatform, LayerMask.GetMask("Platform"));

        if (raySphere.Length <= 0) return t;

        foreach (Collider c in raySphere) {
            if (c.tag == "MovingPlatform" && !c.GetComponentInParent<MovingPlatform>().isActive) {
                MovingPlatform mp = c.GetComponentInParent<MovingPlatform>();
                t.Add(mp);
            }
        }

        return t;
    }

    //private List<StasisController> GetStasisObjects() {
    //    List<StasisController> sc = new List<StasisController>();

    //    Collider[] raySphere = Physics.OverlapSphere(transform.position, maxDistancePlatform);

    //    foreach (Collider c in raySphere) {
    //        StasisController s = c.GetComponent<StasisController>();

    //        if (s != null) {
    //            sc.Add(s);
    //        }
    //    }

    //    return sc;
    //}

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {
        pInDistance = GetPlatformColliders();

        foreach (MovingPlatform t in activatedPlatforms)
        {
            t.stop = false;
        }

        for (int i = 0; i < activatedPlatforms.Count; i++)
        {
            if (!activatedPlatforms[i].isActive)
            {
                activatedPlatforms.Remove(activatedPlatforms[i]);
                i--;
            }
        }
    }

    protected override void UpdateLimbUI() {
        switch (chargeState) {
            case Enums.ChargeState.NOT_CHARGED:
                limbText.text = "";
                break;
            case Enums.ChargeState.TIER_ONE:
                if (pInDistance.Count > 0) {
                    limbText.text = "Send Energy";
                } else {
                    limbText.text = "";
                }
                break;
            case Enums.ChargeState.TIER_TWO:
                if (activatedPlatforms.Count > 0) {
                    limbText.text = "Manipulate Energy";
                } else {
                    limbText.text = "";
                }
                break;
            default:
                break;
        }
    }
}