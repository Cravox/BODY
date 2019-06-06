using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Head : Limb {
    [TabGroup("Balancing")]
    public float maxDistancePlatform;
    [TabGroup("Balancing")]
    public float maxDistanceStasis;

    [TabGroup("References")]
    public Animator anim1;
    [TabGroup("References")]
    public Animator anim2;

    [TabGroup("Debugging")]
    List<MovingPlatform> platforms = new List<MovingPlatform>();

    public override int TierOne() {
        List<MovingPlatform> pInDistance = GetPlatformColliders();

        foreach (MovingPlatform t in pInDistance)
        {
            t.platCol.isTrigger = false;
        }

        if (pInDistance.Count > 0)
        {
            anim1.Play("anim", 0, 0);
            return tierCosts[0];
        }

        return 0;
    }

    public override int TierTwo() {
        foreach (MovingPlatform t in platforms)
        {
            t.stop = false;
        }

        if (platforms.Count > 0)
            return tierCosts[1];

        return 0;
    }

    public override int TierThree() {
        List<StasisController> sInDistance = GetStasisObjects();

        foreach (StasisController sc in sInDistance)
        {
            sc.StasisTrigger();
        }

        if (sInDistance.Count > 0)
        {
            anim2.Play("anim", 0, 0);
            return tierCosts[2];
        }

        return 0;
    }

    List<MovingPlatform> GetPlatformColliders()
    {
        List<MovingPlatform> t = new List<MovingPlatform>();

        Collider[] raySphere = Physics.OverlapSphere(transform.position, maxDistancePlatform, LayerMask.GetMask("Platform"));

        foreach(Collider c in raySphere)
        {
            if (c.tag == "MovingPlatform")
            {
                MovingPlatform mp = c.GetComponent<MovingPlatform>();
                t.Add(mp);

                if (!platforms.Contains(mp))
                    platforms.Add(mp);
            }
        }

        return t;
    }

    List<StasisController> GetStasisObjects()
    {
        List<StasisController> sc = new List<StasisController>();

        Collider[] raySphere = Physics.OverlapSphere(transform.position, maxDistancePlatform);

        foreach (Collider c in raySphere)
        {
            StasisController s = c.GetComponent<StasisController>();

            if (s != null)
            {
                sc.Add(s);
            }
        }

        return sc;
    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {

    }

    protected override void UpdateLimbUI() {

    }
}