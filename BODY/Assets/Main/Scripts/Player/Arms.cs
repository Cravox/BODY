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
        Debug.DrawRay(ray.origin, ray.direction*10, Color.red, 1);

        var interactable = false;
        if (isChecking) {
            if (Physics.Raycast(ray, out hit, interactRange, LayerMask.GetMask("Interactable"))) {
                interactUI.SetImageActive(hit.transform.gameObject.tag);
                interactable = true;
                box = hit.transform;
                boxRb = box.GetComponent<Rigidbody>();
            } else {
                interactUI.SetImageInactive();
            }
        }

        return interactable;
    }

    public override int TierOne() {
        int cost = 0;
        if (canInteract && Input.GetButtonDown("ButtonX") && box.CompareTag("Carry")) {
            IsCarrying = true;
            isChecking = false;
            box.parent = playerCont.modelAxis;
            constraint = boxRb.constraints;
            boxRb.constraints = RigidbodyConstraints.FreezeAll;
            box.localPosition = topPosition.localPosition;
            box.GetComponent<CarryBox>().FirstPickUp = true;
            cost = tierCosts[0];
        } else if (Input.GetButtonDown("ButtonX") && IsCarrying) {
            box.localPosition = frontPosition.localPosition;
            box.parent = null;
            isChecking = true;
            IsCarrying = false;
            boxRb.constraints = constraint;
        }

        IsInteracting = IsCarrying;
        playerCont.modelAnim.SetBool("IsPushing", IsCarrying);
        return cost;
    }

    public override int TierTwo() {
        if (canInteract && Input.GetButtonDown("ButtonX") && box.CompareTag("Push")) {
            // push object
        }
        return tierCosts[1];
    }

    public override int TierThree() {
        // tbd
        return tierCosts[2];
    }
}
