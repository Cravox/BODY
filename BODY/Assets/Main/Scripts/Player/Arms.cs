using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField, TabGroup("References")]
    private Image buttonsImage;

    [SerializeField, TabGroup("References")]
    private Sprite[] buttonSprites = new Sprite[2];

    [SerializeField, TabGroup("Debugging")]
    public Transform Box;

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

        if (Box == null && isCarrying) {
            DetachObject();
        }
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
        var interactable = false;

        var sphereObjects = Physics.OverlapSphere(rayTrans.position, interactRange, LayerMask.GetMask("Interactable"));

        if(Physics.SphereCast(rayTrans.position, 1,playerCont.modelAxis.forward, out hit, interactRange, LayerMask.GetMask("Interactable"))){
            interactable = true;
            Box = hit.transform;
            if (Box.CompareTag("Carry")) boxRb = Box.GetComponent<Rigidbody>();
        } else {
            Box = null;
        }

        return interactable;
    }

    public override void BaselineAbility() {
        if (canInteract && Box.CompareTag("Carry")) {
            SoundController.Play(gameObject, SoundController.Sounds.CHAR_PICKUP, 128, 0.5f);
            Box.GetComponent<CarryBox>().playerArms = this;
            playerCont.modelAnim.SetBool("isCarrying", true);
        } else if (isCarrying) {
            if (Box == null)
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
        if (canInteract && !isCarrying && playerCont.isGrounded && Box.GetComponent<PushBox>() != null) {
            GameManager.instance.CanControl = false;
            playerCont.modelAnim.SetTrigger("Push");
            SoundController.Play(gameObject, SoundController.Sounds.CHAR_PUSH, 128, 0.5f);
            cost = tierCosts[1];
        }
        return cost;
    }

    public void PushBoxEvent() {
        pushBox = Box.GetComponent<PushBox>();
        pushBox.PushedBox(transform.position, pushForce);
    }

    public void ThrowBoxEvent() {
        var modelTrans = playerCont.modelAxis.transform;
        boxRb.AddForce(modelTrans.forward * throwForce + modelTrans.up * upForce);
        DetachObject();
    }

    public override int TierThree() {
        int cost = 0;
        if (canInteract && !isCarrying) {
            PushBox pushBox = Box.GetComponent<PushBox>();
            pushBox.PushedBox(transform.position, pushForce);
            cost = tierCosts[2];
        }
        return cost;
    }

    protected override void UpdateLimbUI() {
        if(Input.GetAxis("LeftTrigger") >= 0.9f) {
            buttonsImage.sprite = buttonSprites[1];
        } else {
            buttonsImage.sprite = buttonSprites[0];
        }

        if (canInteract) {
            if(Box.CompareTag("Carry")) {
                limbImage.sprite = actionSprite[0];
            } else {
                limbImage.sprite = actionSprite[3];
            }
        }

        if(isCarrying && Input.GetAxis("LeftTrigger") >= 0.9f) {
            limbImage.sprite = actionSprite[2];
        } else if (isCarrying) {
            limbImage.sprite = actionSprite[1];
        }

        if(!canInteract && !isCarrying) {
            limbImage.enabled = false;
        } else {
            limbImage.enabled = true;
        }
    }

    public void AttachObject() {
        if(Box != null) {
            isCarrying = true;
            Box.GetComponent<BoxCollider>().isTrigger = true;
            Box.parent = topPosition;
            //constraint = boxRb.constraints;
            boxRb.constraints = RigidbodyConstraints.FreezeAll;
            Box.localRotation = Quaternion.identity;
            Box.localPosition = Vector3.zero;
            Box.GetComponent<CarryBox>().gettingCarried = true;
        }
    }

    public void DetachObject() {
        if (Box != null && boxRb != null) {
            Box.GetComponent<BoxCollider>().isTrigger = false;
            playerCont.modelAnim.SetBool("isCarrying", false);
            Box.parent = null;
            Box.GetComponent<CarryBox>().playerArms = null;
            boxRb.constraints = RigidbodyConstraints.None;
            Box.GetComponent<CarryBox>().gettingCarried = false;
        }

        isCarrying = false;
    }

    public override void InputCheck() {
        predictLine.enabled = (Input.GetAxis("LeftTrigger") >= 0.9f && isCarrying);

        if (Input.GetButtonDown("ButtonX") && Box != null) {
            if (Box.GetComponent<PushBox>() != null && Input.GetAxis("LeftTrigger") <= 0.1f) {
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
