using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Arms : SerializedMonoBehaviour, IPlayerLimb {
    public PlayerController playerCont => throw new System.NotImplementedException();

    public bool FullyCharged { get { return energyState == Enums.EnergyStates.FULLY_CHARGED; } }

    public Enums.EnergyStates EnergyState { get { return this.energyState; } set { energyState = value; } }
    private Enums.EnergyStates energyState = Enums.EnergyStates.NOT_CHARGED;

    public Enums.Limb Limb { get { return limb; } }
    private Enums.Limb limb = Enums.Limb.ARMS;


    public void Charge() {
        energyState++;
        print(energyState);
    }

    public void Discharge() {
        energyState = Enums.EnergyStates.NOT_CHARGED;
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
