using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Arms : Limb {
    [SerializeField, TabGroup("Balancing")]
    private float interactRange = 1;

    [SerializeField, TabGroup("Balancing")]
    private float throwForce = 5;

    [SerializeField, TabGroup("Balancing")]
    private float upForce = 5;

    [SerializeField, TabGroup("Balancing")]
    private float pushForce = 1000;

    [SerializeField, TabGroup("Balancing")]
    private int impactIterations;

    [SerializeField, TabGroup("Balancing")]
    private float impactIterationLenght;

    [SerializeField, TabGroup("References"), Required]
    private Transform topPosition;

    [SerializeField, TabGroup("References"), Required]
    private Transform frontPosition;

    [SerializeField, TabGroup("References"), Required]
    private Transform rayTrans;

    [SerializeField, TabGroup("References"), Required]
    private Transform ball;

    [SerializeField, TabGroup("Debugging")]
    private Transform box;

    [SerializeField, TabGroup("Debugging")]
    private LayerMask indicatorMask;

    private bool isCarrying;

    private Ray ray;
    private RaycastHit hit;
    private RigidbodyConstraints constraint;
    private Rigidbody boxRb;

    private bool isChecking = true;
    private bool canInteract;
    private Vector3 impactPos;

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {
        if (!isCarrying) {
            canInteract = CheckForInteractable();
        } else {
            if (chargeState == Enums.ChargeState.TIER_ONE) PredictRigidbody();
            canInteract = false;
        }
    }

    public void PredictRigidbody() {
        if (!isCarrying) {
            impactPos = Vector3.zero;
            return;
        }

        Vector3 dir = playerCont.modelAxis.forward;
        float vFwd = (throwForce / boxRb.mass) * Time.fixedDeltaTime;
        float vUp = (upForce / boxRb.mass) * Time.fixedDeltaTime;

        impactPos = CalcRigidPos(topPosition.position, dir, vFwd, vUp, impactIterations, impactIterationLenght);
        ball.position = impactPos;
    }

    Vector3 CalcRigidPos(Vector3 origin, Vector3 dir, float fwd, float up, int iterations, float stepDistance) {
        Vector3 result = Vector3.zero;
        List<Vector3> calcs = new List<Vector3>();
        calcs.Add(origin);

        for (int i = 1; i < iterations + 1; i++) {
            float t = i * stepDistance;
            Vector3 pos = origin + ((Vector3.up * up * t) + (dir * fwd * t) + Physics.gravity / 2 * t * t);
            calcs.Add(pos);

            RaycastHit hit;
            bool cast = Physics.Linecast(calcs[i - 1], calcs[i], out hit, indicatorMask);

            Debug.DrawLine(calcs[i - 1], calcs[i], new Color(1 / i * 10, 1 / i * 10, 1 / i * 10));
            

            if (hit.collider != null) {
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
        } else if (isCarrying) {
            box.localPosition = frontPosition.localPosition;
            DetachObject();
        }

        playerCont.modelAnim.SetBool("IsPushing", isCarrying);
    }

    public override int TierOne() {
        int cost = 0;
        if (isCarrying) {
            DetachObject();

            var modelTrans = playerCont.modelAxis.transform;

            boxRb.AddForce(modelTrans.forward * throwForce + modelTrans.up * upForce);
            cost = tierCosts[0];
        }
        playerCont.modelAnim.SetBool("IsPushing", isCarrying);
        return cost;
    }

    public override int TierTwo() {
        int cost = 0;
        if (canInteract && !isCarrying) {
            PushBox pushBox = box.GetComponent<PushBox>();
            pushBox.PushedBox(transform.position, pushForce);
            cost = tierCosts[1];
        }
        return cost;
    }

    public override int TierThree() {
        int cost = 0;
        if (canInteract && !isCarrying) {
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
        } else if (isCarrying) {
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
        isCarrying = true;
        box.parent = playerCont.modelAxis;
        constraint = boxRb.constraints;
        boxRb.constraints = RigidbodyConstraints.FreezeAll;
        box.localPosition = topPosition.localPosition;
        box.GetComponent<CarryBox>().FirstPickUp = true;
    }

    private void DetachObject() {
        box.parent = null;
        isCarrying = false;
        boxRb.constraints = constraint;
    }
}
