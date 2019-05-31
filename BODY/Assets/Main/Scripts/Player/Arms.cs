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

    protected override string limbName => "Arms";

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {

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

    public override void TierOne() {
        canInteract = CheckForInteractable();

        if (EnergySystem.chargeState == EnergySystem.ChargeState.NOT_CHARGING) {
            if (canInteract && Input.GetButtonDown("ButtonX") && box.CompareTag("Carry")) {
                IsCarrying = true;
                isChecking = false;
                box.parent = playerCont.modelAxis;
                constraint = boxRb.constraints;
                boxRb.constraints = RigidbodyConstraints.FreezeAll;
                box.localPosition = topPosition.localPosition;
                box.GetComponent<CarryBox>().FirstPickUp = true;
            } else if (Input.GetButtonDown("ButtonX") && IsCarrying) {
                box.localPosition = frontPosition.localPosition;
                box.parent = null;
                isChecking = true;
                IsCarrying = false;
                boxRb.constraints = constraint;
            }
        }

        IsInteracting = IsCarrying;
        playerCont.modelAnim.SetBool("IsPushing", IsCarrying);
    }

    public override void TierTwo() {
        if (canInteract && Input.GetButtonDown("ButtonX") && box.CompareTag("Push")) {
            // push object
        }
    }

    public override void TierThree() {
        // tbd
    }

    protected override void OnDischarge() {
        switch (EnergyState) {
            case Enums.EnergyStates.ZERO_CHARGES:
                interactUI.SetImageInactive();
                if (IsCarrying) {
                    box.localPosition = frontPosition.localPosition;
                    box.parent = null;
                    IsCarrying = false;
                    IsInteracting = false;
                    isChecking = true;
                    boxRb.constraints = constraint;
                    playerCont.modelAnim.SetBool("IsPushing", false);
                }
                break;
            case Enums.EnergyStates.THREE_CHARGES:
                break;
            case Enums.EnergyStates.SIX_CHARGES:
                break;
            case Enums.EnergyStates.FULLY_CHARGED:
                break;
            default:
                break;
        }
    }
}
