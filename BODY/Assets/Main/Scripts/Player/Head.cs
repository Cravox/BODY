using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Head : Limb {
    [TabGroup("Balancing")]
    public float maxDistancePlatform;
    [TabGroup("Balancing")]
    public float maxDistanceStasis;

    [SerializeField, TabGroup("Balancing")]
    private float maxStasisTime;

    [TabGroup("References")]
    public Animator anim1;
    [TabGroup("References")]
    public Animator anim2;

    [TabGroup("Debugging"), SerializeField]
    private List<MovingPlatform> activatedPlatforms = new List<MovingPlatform>();

    private List<MovingPlatform> pInDistance;

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
        List<StasisController> sInDistance = GetStasisObjects();

        anim2.Play("anim", 0, 0);

        foreach (StasisController sc in sInDistance) {
            sc.StartCoroutine(sc.Stasis(maxStasisTime));
        }

        if (sInDistance.Count > 0) {
            return tierCosts[2];
        }

        playerCont.modelAnim.Play("Dance");

        return 0;
    }

    private List<MovingPlatform> GetPlatformColliders() {
        List<MovingPlatform> t = new List<MovingPlatform>();

        Collider[] raySphere = Physics.OverlapSphere(transform.position, maxDistancePlatform, LayerMask.GetMask("Platform"));

        if (raySphere.Length <= 0) return t;

        foreach (Collider c in raySphere) {
            if (c.tag == "MovingPlatform" && !c.GetComponent<MovingPlatform>().isActive) {
                MovingPlatform mp = c.GetComponent<MovingPlatform>();
                t.Add(mp);

                //if (!platforms.Contains(mp))
                //    platforms.Add(mp);
            }
        }

        return t;
    }

    private List<StasisController> GetStasisObjects() {
        List<StasisController> sc = new List<StasisController>();

        Collider[] raySphere = Physics.OverlapSphere(transform.position, maxDistancePlatform);

        foreach (Collider c in raySphere) {
            StasisController s = c.GetComponent<StasisController>();

            if (s != null) {
                sc.Add(s);
            }
        }

        return sc;
    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {
        pInDistance = GetPlatformColliders();
    }

    protected override void UpdateLimbUI() {
        switch (chargeState) {
            case Enums.ChargeState.TIER_ONE:
                if (pInDistance.Count > 0) {
                    limbText.text = "Send Energy";
                } else {
                    limbText.text = "";
                }
                break;
            case Enums.ChargeState.TIER_TWO:
                if(activatedPlatforms.Count > 0) {
                    limbText.text = "Manipulate Energy";
                } else {
                    limbText.text = "";
                }
                break;
            case Enums.ChargeState.TIER_THREE:
                limbText.text = "Stasis";
                break;
            default:
                break;
        }
    }
}