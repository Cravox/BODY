using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Arms : Limb {
    [SerializeField]
    private InteractableUI interactUI;

    [SerializeField]
    private float interactRange = 1;

    [SerializeField, TabGroup("References")]
    private Transform topPosition;

    [SerializeField, TabGroup("References")]
    private Transform frontPosition;

    private Ray ray;
    private RaycastHit hit;
    private RigidbodyConstraints constraint;
    private Rigidbody boxRb;

    private Transform box;

    [HideInInspector]
    public bool isPushing;

    protected override string limbName => "Arms";

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
                boxRb = box.GetComponent<Rigidbody>();
                if (Input.GetButtonDown("ButtonX")) {
                    isPushing = true;
                    box.parent = playerCont.modelAxis;
                    constraint = boxRb.constraints;
                    boxRb.constraints = RigidbodyConstraints.FreezeAll;
                    box.localPosition = topPosition.localPosition;
                }
            } else {
                interactUI.SetImageActive(false);
            }
        } else {
            if (Input.GetButtonDown("ButtonX")) {
                box.localPosition = frontPosition.localPosition;
                box.parent = null;
                isPushing = false;
                boxRb.constraints = constraint;
            }
        }
        playerCont.modelAnim.SetBool("IsPushing", isPushing);
    }

    public override void TierTwo() {

    }

    public override void TierThree() {

    }

    protected override void OnDeactivation() {
        interactUI.SetImageActive(false);
        if (isPushing)
        {
            box.localPosition = frontPosition.localPosition;
            box.parent = null;
            isPushing = false;
            boxRb.constraints = constraint;
            playerCont.modelAnim.SetBool("IsPushing", false);
        }
    }
}
