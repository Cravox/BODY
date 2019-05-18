using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Arms : Limb {
    [SerializeField]
    private InteractableUI interactUI;

    [SerializeField]
    public float interactRange = 1;

    Ray ray;
    RaycastHit hit;

    private Transform box;

    [HideInInspector]
    public bool isPushing;

    public override int index => 1;

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {

    }

    public override void TierOne() {
        ray.origin = transform.position;
        ray.direction = playerCont.modelAxis.forward;
        Debug.DrawRay(ray.origin, ray.direction*10, Color.red, 1);

        if (!isPushing) {
            if (Physics.Raycast(ray, out hit, interactRange, LayerMask.GetMask("ArmBox"))) {
                interactUI.SetImageActive(true);
                box = hit.transform;
                if (Input.GetButtonDown("ButtonX")) {
                    isPushing = true;
                    box.parent = playerCont.modelAxis;
                }
            } else {
                interactUI.SetImageActive(false);
            }
        } else {
            if (Input.GetButtonDown("ButtonX")) {
                box.parent = null;
                isPushing = false;
            }
        }
        playerCont.modelAnim.SetBool("IsPushing", isPushing);
    }

    public override void TierTwo() {

    }

    public override void TierThree() {

    }
}
