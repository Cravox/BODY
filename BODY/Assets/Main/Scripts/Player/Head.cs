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
    public List<MovingPlatform> activatedPlatforms = new List<MovingPlatform>();

    public List<MovingPlatform> pInDistance;

    public override void BaselineAbility() {
        if (GameManager.instance.aktPuzzle != null) {
            GameManager.instance.camImage.enabled = !GameManager.instance.camImage.enabled;
        }
    }

    public override int TierOne() {
        anim1.Play("anim", 0, 0);

        foreach (MovingPlatform platform in pInDistance) {
            platform.platCol.isTrigger = false;
            activatedPlatforms.Add(platform);
            platform.animate.SetTrigger("Triggered");
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

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {
        pInDistance = GetPlatformColliders();
    }

    protected override void UpdateLimbUI() {

    }

    public override void InputCheck() {
        if (Input.GetButtonDown("ButtonY") && Input.GetAxis("LeftTrigger") >= 0.9f) {
            TierTwo();
        } else if (Input.GetButtonDown("ButtonY")) {
            BaselineAbility();
        }
    }
}