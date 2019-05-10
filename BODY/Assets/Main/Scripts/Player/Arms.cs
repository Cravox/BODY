using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Arms : SerializedMonoBehaviour, IPlayerLimb {
    public PlayerController playerCont { get { return PlayerController.instance; } }

    public bool FullyCharged { get { return energyState == Enums.EnergyStates.FULLY_CHARGED; } }

    public Enums.EnergyStates EnergyState { get { return this.energyState; } set { energyState = value; } }
    private Enums.EnergyStates energyState = Enums.EnergyStates.ZERO_CHARGES;

    public Enums.Limb Limb { get { return limb; } }
    private Enums.Limb limb = Enums.Limb.ARMS;

    [SerializeField]
    private InteractableUI interactUI;

    [SerializeField]
    public float interactRange = 1;

    Ray ray;
    RaycastHit hit;

    private Transform box;

    [HideInInspector]
    public bool isPushing;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        switch (energyState) {
            case Enums.EnergyStates.ZERO_CHARGES:
                break;
            case Enums.EnergyStates.ONE_CHARGE:
                TierOne();
                break;
            case Enums.EnergyStates.TWO_CHARGES:
                TierOne();
                TierTwo();
                break;
            case Enums.EnergyStates.FULLY_CHARGED:
                break;
            default:
                break;
        }
    }

    public void Charge() {
        energyState++;
    }

    public void Discharge() {
        energyState = Enums.EnergyStates.ZERO_CHARGES;
    }

    public void TierOne() {
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
            } else interactUI.SetImageActive(false);
        } else {
            if (Input.GetButtonDown("ButtonX")) {
                box.parent = null;
                isPushing = false;
            }
        }
        playerCont.modelAnim.SetBool("IsPushing", isPushing);
    }

    public void TierThree() {

    }

    public void TierTwo() {

    }
}
