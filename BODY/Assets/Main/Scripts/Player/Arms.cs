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


    public Vector3 impactPos;
    public int impactIterations;
    public float impactIterationLenght;
    public Transform ball;
    public LayerMask rMask;

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {
        if (!IsCarrying) {
            canInteract = CheckForInteractable();
        } else {
            canInteract = false;
        }

        PredictRigidbody();
    }

    public void PredictRigidbody()
    {
        if(!IsCarrying)
        {
            impactPos = Vector3.zero;
            return;
        }

        Vector3 dir = playerCont.modelAxis.forward;
        float vFwd = (pushForce / boxRb.mass);
        float vUp = (upForce / boxRb.mass);

        impactPos = CalcRigidPos(topPosition.position, dir, vFwd, vUp, impactIterations, impactIterationLenght);
        ball.position = impactPos;
    }

    Vector3 CalcRigidPos(Vector3 origin, Vector3 dir, float fwd, float up, int iterations, float stepDistance)
    {
        Vector3 result = Vector3.zero;
        List<Vector3> calcs = new List<Vector3>();
        calcs.Add(origin);

        for (int i = 1; i < iterations + 1; i++)
        {
            Vector3 pos = origin + (dir * fwd * (i * stepDistance)) + (Vector3.up * up) + Physics.gravity * (i * stepDistance * 180);
            calcs.Add(pos);

            RaycastHit hit;
            bool cast = Physics.Linecast(calcs[i-1], calcs[i], out hit, rMask);

            if (hit.collider != null)
            {
                result = hit.point;
                return result;
            }
        }

        return result;
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
