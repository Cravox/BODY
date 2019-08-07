using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class Arms : Limb {
    [SerializeField, TabGroup("Balancing")]
    private float interactRange = 1;

    [SerializeField, TabGroup("Balancing")]
    private float throwForce = 5;

    [SerializeField, TabGroup("Balancing")]
    private float upForce = 5;

    [SerializeField, TabGroup("Balancing")]
    private float pushForce = 1000;

    [SerializeField, TabGroup("References"), Required]
    private Transform topPosition;

    [SerializeField, TabGroup("References"), Required]
    private Transform frontPosition;

    [SerializeField, TabGroup("References"), Required]
    private Transform rayTrans;

    [SerializeField, TabGroup("References"), Required]
    private LineRenderer predictLine;

    [SerializeField, TabGroup("Debugging")]
    private Transform box;

    [SerializeField, TabGroup("Debugging")]
    private LayerMask indicatorMask;

    [SerializeField, TabGroup("Debugging")]
    private bool isCarrying;

    [SerializeField, TabGroup("Debugging"), Header("Range Indicator")]
    private int predictLineIterations;

    [SerializeField, TabGroup("Debugging")]
    private float predictLineLength;

    private LineRenderer line;

    private PushBox pushBox;

    private Ray ray;
    private RaycastHit hit;
    private RigidbodyConstraints constraint;
    private Rigidbody boxRb;

    private bool canInteract;
    public List<Vector3> linePos;
    private Rigidbody lastRb;

    RigidbodyConstraints constraints;

    protected override void LimbStart() {
        
    }

    protected override void LimbUpdate() {
        SetPredictLine();

        if (!playerCont.animEvents.isPushing) {
            playerCont.rigid.constraints = constraints;
        }

        if (!isCarrying) {
            canInteract = CheckForInteractable();
        } else {
            canInteract = false;
        }

        if (box == null && isCarrying) {
            DetachObject();
        }

        //playerCont.modelAnim.SetBool("isCarrying", isCarrying);
    }

    public void SetPredictLine() {
        if (!isCarrying || boxRb == null || boxRb == lastRb)
            return;

        float vFwd = (throwForce / boxRb.mass) * Time.fixedDeltaTime;
        float vUp = (upForce / boxRb.mass) * Time.fixedDeltaTime;
        linePos = CalcPredictPos(vFwd, vUp, predictLineIterations, predictLineLength);

        predictLine.positionCount = linePos.Count;
        predictLine.SetPositions(linePos.ToArray());

        lastRb = boxRb;
    }

    List<Vector3> CalcPredictPos(float fwd, float up, int iterations, float stepDistance) {
        List<Vector3> calcs = new List<Vector3>();
        calcs.Add(Vector3.zero);

        for (int i = 1; i < iterations + 1; i++) {
            float t = i * stepDistance;
            Vector3 pos = new Vector3();
            pos = (Vector3.up * up * t) + (Vector3.forward * fwd * t) + Physics.gravity / 2 * t * t;
            calcs.Add(pos);
        }

        return calcs;
    }

    private bool CheckForInteractable() {
        ray.origin = rayTrans.position;
        ray.direction = playerCont.modelAxis.forward;

        //var sphereObjects = Physics.OverlapSphere(rayTrans.position, interactRange, LayerMask.GetMask("Interactable"));
        //
        //if (sphereObjects.Length == 0) {
        //    box = null;
        //    return false;
        //}
        //
        //sphereObjects.OrderBy(so => Vector3.Distance(so.transform.position, this.transform.position));
        //
        //box = sphereObjects[0].transform;
        //if (box.CompareTag("Carry")) boxRb = box.GetComponent<Rigidbody>();
        var interactable = false;
        if (Physics.Raycast(ray, out hit, interactRange, LayerMask.GetMask("Interactable"))) {
            interactable = true;
            box = hit.transform;
            if (box.CompareTag("Carry")) boxRb = box.GetComponent<Rigidbody>();
        } else {
            box = null;
        }

        return interactable;
    }

    public override void BaselineAbility() {
        if (canInteract && box.CompareTag("Carry")) {
            SoundController.Play(gameObject, SoundController.Sounds.CHAR_PICKUP, 128, 0.5f);
            box.GetComponent<CarryBox>().playerArms = this;
            playerCont.modelAnim.SetBool("isCarrying", true);
        } else if (isCarrying) {
            if (box == null)
                isCarrying = false;
            else {
                playerCont.modelAnim.SetBool("isCarrying", false);
            }
        }
    }

    public override int TierOne() {
        int cost = 0;
        if (isCarrying) {
            playerCont.modelAnim.SetTrigger("Throw");
            SoundController.Play(gameObject, SoundController.Sounds.CHAR_THROW, 128, 0.5f);
            cost = tierCosts[0];
        } else {
            DetachObject();
        }
        return cost;
    }

    public override int TierTwo() {
        int cost = 0;
        if (canInteract && !isCarrying && playerCont.isGrounded && box.GetComponent<PushBox>() != null) {
            GameManager.instance.CanControl = false;
            playerCont.modelAnim.SetTrigger("Push");
            SoundController.Play(gameObject, SoundController.Sounds.CHAR_PUSH, 128, 0.5f);
            cost = tierCosts[1];
        }
        return cost;
    }

    public void PushBoxEvent() {
        pushBox = box.GetComponent<PushBox>();
        pushBox.PushedBox(transform.position, pushForce);
        //pushBox = null;
    }

    public void ThrowBoxEvent() {
        var modelTrans = playerCont.modelAxis.transform;
        boxRb.AddForce(modelTrans.forward * throwForce + modelTrans.up * upForce);
        DetachObject();
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

    public void AttachObject() {
        isCarrying = true;
        //var saveRota = box.localRotation;
        if(box != null) {
            box.GetComponent<BoxCollider>().isTrigger = true;
            box.parent = topPosition;
            constraint = boxRb.constraints;
            boxRb.constraints = RigidbodyConstraints.FreezeAll;
            box.localRotation = Quaternion.identity;
            box.localPosition = Vector3.zero;
        }
    }

    public void DetachObject() {
        if (box != null && boxRb != null) {
            box.GetComponent<BoxCollider>().isTrigger = false;
            playerCont.modelAnim.SetBool("isCarrying", false);
            box.parent = null;
            box.GetComponent<CarryBox>().playerArms = null;
            boxRb.constraints = constraint;
        }

        isCarrying = false;
    }

    public override void InputCheck() {
        predictLine.enabled = (Input.GetAxis("LeftTrigger") >= 0.9f && isCarrying);

        if (Input.GetButtonDown("ButtonX") && box != null) {
            if (box.GetComponent<PushBox>() != null) {
                TierTwo();
            } else {
                if (isCarrying && Input.GetAxis("LeftTrigger") >= 0.9f) {
                    TierOne();
                } else if (!isCarrying) {
                    BaselineAbility();
                } else {
                    BaselineAbility();
                }
            }
        }
    }
}
