using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Arms : Limb {
    [SerializeField]
    private float interactRange = 1;

    [SerializeField, TabGroup("References")]
    private Transform topPosition;

    [SerializeField, TabGroup("References")]
    private Transform frontPosition;

    [SerializeField, TabGroup("Balancing")]
    private float throwForce = 5;

    [SerializeField, TabGroup("Balancing")]
    private float upForce = 5;

    [SerializeField, TabGroup("Balancing")]
    private float pushForce = 1000;

    [SerializeField, TabGroup("References")]
    private Transform rayTrans;

    [SerializeField, TabGroup("Debugging")]
    private Transform box;

    private Ray ray;
    private RaycastHit hit;
    private RigidbodyConstraints constraint;
    private Rigidbody boxRb;

    [HideInInspector]
    public bool IsCarrying;

    private bool isChecking = true;
    private bool canInteract;

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {
        if (!IsCarrying) {
            canInteract = CheckForInteractable();
        } else {
            canInteract = false;
        }
    }

    private bool CheckForInteractable() {
        ray.origin = rayTrans.position;
        ray.direction = playerCont.modelAxis.forward;

        var interactable = false;
        if (Physics.Raycast(ray, out hit, interactRange, LayerMask.GetMask("Interactable"))) {
            interactable = true;
            box = hit.transform;
            boxRb = box.GetComponent<Rigidbody>();
        }

        return interactable;
    }

    public override void BaselineAbility() {
        if (canInteract && box.CompareTag("Carry")) {
            AttachObject();
        } else if (IsCarrying) {
            box.localPosition = frontPosition.localPosition;
            DetachObject();
        }

        playerCont.modelAnim.SetBool("IsPushing", IsCarrying);
    }

    public override int TierOne() {
        int cost = 0;
        if (IsCarrying) {
            DetachObject();

            var modelTrans = playerCont.modelAxis.transform;

            boxRb.AddForce(modelTrans.forward * throwForce + modelTrans.up * upForce);
            cost = tierCosts[0];
        }
        playerCont.modelAnim.SetBool("IsPushing", IsCarrying);
        return cost;
    }

    public override int TierTwo() {
        int cost = 0;
        if (canInteract && !IsCarrying) {
            PushBox pushBox = box.GetComponent<PushBox>();
            pushBox.PushedBox(transform.position, pushForce);
            cost = tierCosts[1];
        }
        return cost;
    }

    public override int TierThree() {
        int cost = 0;
        if(canInteract && !IsCarrying) {
            PushBox pushBox = box.GetComponent<PushBox>();
            pushBox.PushedBox(transform.position, pushForce);
            cost = tierCosts[2];
        }
        return cost;
    }

    protected override void UpdateLimbUI() {
        if (canInteract && box.CompareTag("Carry") && chargeState == Enums.ChargeState.NOT_CHARGED) {
            limbText.text = "Pick up";
        } else if (canInteract && box.CompareTag("Push") && chargeState == Enums.ChargeState.TIER_TWO) {
            limbText.text = "Push";
        } else if (IsCarrying) {
            switch (chargeState) {
                case Enums.ChargeState.NOT_CHARGED:
                    limbText.text = "Drop";
                    break;
                case Enums.ChargeState.TIER_ONE:
                    limbText.text = "Throw";
                    break;
                case Enums.ChargeState.TIER_TWO:
                    limbText.text = "";
                    break;
                default:
                    break;
            }
        } else limbText.text = "";
    }

    private void AttachObject() {
        IsCarrying = true;
        box.parent = playerCont.modelAxis;
        constraint = boxRb.constraints;
        boxRb.constraints = RigidbodyConstraints.FreezeAll;
        box.localPosition = topPosition.localPosition;
        box.GetComponent<CarryBox>().FirstPickUp = true;
    }

    private void DetachObject() {
        box.parent = null;
        IsCarrying = false;
        boxRb.constraints = constraint;
    }
}
