using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour, IPlayerLimb {
    public PlayerController playerCont { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool FullyCharged { get { return energyState == EnergyStates.FULLY_CHARGED; } }

    public EnergyStates EnergyState { get { return this.energyState; } set { energyState = value; } }
    private EnergyStates energyState = EnergyStates.NOT_CHARGED;

    public void Charge() {
        if (FullyCharged) {
            print("Head fully charged");
            return;
        }
        energyState++;
        print(energyState);
    }

    public void Discharge() {
        energyState = EnergyStates.NOT_CHARGED;
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
