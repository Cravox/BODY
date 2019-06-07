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

    [SerializeField, TabGroup("Balancing")]
    private float throwForce = 5;

    [SerializeField, TabGroup("Balancing")]
    private float upForce = 5;

    [SerializeField, TabGroup("Balancing")]
    private float pushForce = 1000;

    private Ray ray;
    private RaycastHit hit;
    private RigidbodyConstraints constraint;
    private Rigidbody boxRb;

    private Transform box;

    [HideInInspector]
    public bool IsCarrying;

    private bool isChecking = true;
    private bool canInteract;

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {
        canInteract = CheckForInteractable();
    }

    private bool CheckForInteractable() {
        ray.origin = transform.position;
        ray.direction = playerCont.modelAxis.forward;
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red, 1);

        var interactable = false;
        if (Physics.Raycast(ray, out hit, interactRange, LayerMask.GetMask("Interactable"))) {
            interactable = true;
            box = hit.transform;
            boxRb = box.GetComponent<Rigidbody>();
        }

        return interactable;
    }

    public override int TierOne() {
        playerCont.rigid.velocity = Vector3.zero;
        int cost = 0;
        if (canInteract && box.CompareTag("Carry")) {
            AttachObject();
            cost = tierCosts[0];
        } else if (IsCarrying) {
            box.localPosition = frontPosition.localPosition;
            DetachObject();
        }

        IsInteracting = IsCarrying;
        playerCont.modelAnim.SetBool("IsPushing", IsCarrying);
        return cost;
    }

    public override int TierTwo() {
        playerCont.rigid.velocity = Vector3.zero;
        int cost = 0;
        if (IsCarrying) {
            DetachObject();

            var modelTrans = playerCont.modelAxis.transform;

            boxRb.AddForce(modelTrans.forward * throwForce + modelTrans.up * upForce);
            cost = tierCosts[1];
        }
        playerCont.modelAnim.SetBool("IsPushing", IsCarrying);
        return cost;
    }

    public override int TierThree() {
        // tbd
        boxRb.AddForce(playerCont.modelAxis.transform.forward * pushForce);
        return tierCosts[2];
    }

    protected override void UpdateLimbUI() {
        if (canInteract && box.CompareTag("Carry") && chargeState == Enums.ChargeState.TIER_ONE) {
            limbText.text = "Pick up";
        } else if (canInteract && box.CompareTag("Push") && chargeState == Enums.ChargeState.TIER_THREE) {
            limbText.text = "Push";
        } else if (IsCarrying) {
            switch (chargeState) {
                case Enums.ChargeState.TIER_ONE:
                    limbText.text = "Drop";
                    break;
                case Enums.ChargeState.TIER_TWO:
                    limbText.text = "Throw";
                    break;
                case Enums.ChargeState.TIER_THREE:
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
