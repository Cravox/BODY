using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

// head, arms and legs inherit from limb
[RequireComponent(typeof(PlayerController))]
public abstract class Limb : SerializedMonoBehaviour {
    [SerializeField, TabGroup("References"), Required]
    protected TextMeshProUGUI limbText;

    [HideInInspector]
    public Enums.ChargeState chargeState;

    protected PlayerController playerCont;

    [SerializeField, TabGroup("Balancing"), Tooltip("From top to bottom: TierOne, TierTwo, TierThree")]
    protected int[] tierCosts = new int[3];

    protected void Start() {
        playerCont = GetComponent<PlayerController>();
        LimbStart();
    }

    protected void Update() {
        LimbUpdate();
        UpdateLimbUI();
    }

    public abstract void BaselineAbility();
    public abstract int TierOne();
    public abstract int TierTwo();
    public abstract int TierThree();

    protected abstract void UpdateLimbUI();
    protected abstract void LimbUpdate();
    protected abstract void LimbStart();
}
