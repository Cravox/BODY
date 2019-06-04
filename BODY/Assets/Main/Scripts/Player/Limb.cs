using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

// head, arms and legs inherit from limb
[RequireComponent(typeof(PlayerController))]
public abstract class Limb : SerializedMonoBehaviour {
    protected PlayerController playerCont;
    public bool FullyCharged { get; set; }
    public bool IsInteracting;

    [SerializeField, TabGroup("Balancing"), Tooltip("From top to bottom: TierOne, TierTwo, TierThree")]
    protected int[] tierCosts = new int[3];

    protected void Start() {
        playerCont = GetComponent<PlayerController>();
        LimbStart();
    }

    protected void Update() {
        LimbUpdate();
    }

    public abstract int TierOne();
    public abstract int TierTwo();
    public abstract int TierThree();

    protected abstract void LimbUpdate();
    protected abstract void LimbStart();
}
