using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Head : SerializedMonoBehaviour, IPlayerLimb {
    public PlayerController playerCont { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool FullyCharged { get { return energyState == Enums.EnergyStates.FULLY_CHARGED; } }

    public Enums.EnergyStates EnergyState { get { return this.energyState; } set { energyState = value; } }
    private Enums.EnergyStates energyState = Enums.EnergyStates.NOT_CHARGED;

    public Enums.Limb Limb { get { return limb; } }
    private Enums.Limb limb = Enums.Limb.HEAD;


    public void Charge() {
        energyState++;
        print(energyState);
    }

    public void Discharge() {
        energyState = Enums.EnergyStates.NOT_CHARGED;
        print("Discharge Head");
    }

    public void TierOne() {

    }

    public void TierThree() {

    }

    public void TierTwo() {

    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
