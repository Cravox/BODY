using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Arms : SerializedMonoBehaviour, IPlayerLimb {
    public PlayerController playerCont => throw new System.NotImplementedException();

    public bool FullyCharged { get { return energyState == EnergyStates.FULLY_CHARGED; } }

    public EnergyStates EnergyState { get { return this.energyState; } set { energyState = value; } }
    private EnergyStates energyState = EnergyStates.NOT_CHARGED;

    public void Charge() {
        if (FullyCharged) {
            print("Arms fully charged");
            return;
        }
        energyState++;
        print(energyState);
    }

    public void Discharge() {
        energyState = EnergyStates.NOT_CHARGED;
        print("Discharge Arms");
    }

    public void TierOne() {
        throw new System.NotImplementedException();
    }

    public void TierThree() {
        throw new System.NotImplementedException();
    }

    public void TierTwo() {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
