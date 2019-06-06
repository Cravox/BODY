using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Head : Limb {


    List<MovingPlatform> platforms = new List<MovingPlatform>();
    public float maxDistance;

    public override int TierOne() {
        List<MovingPlatform> pInDistance = GetPlatformColliders();

        foreach (MovingPlatform t in pInDistance)
        {
            t.platCol.isTrigger = false;
        }
        
        if(pInDistance.Count > 0)
            return tierCosts[0];

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
        //stasis
        return 0;
    }

    List<MovingPlatform> GetPlatformColliders()
    {
        List<MovingPlatform> t = new List<MovingPlatform>();

        Collider[] raySphere = Physics.OverlapSphere(transform.position, maxDistance, LayerMask.GetMask("Platform"));

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

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {

    }
}