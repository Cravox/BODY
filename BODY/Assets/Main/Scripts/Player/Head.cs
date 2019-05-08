using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour, IPlayerLimb
{
    public PlayerController playerCont { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public bool fullyCharged { get { return (energyState == EnergyState.FULLY_CHARGED); } set { ; } }
    public EnergyState energyState { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Charge() {

    }

    public void Discharge() {

    }

    public void TierOne() {

    }

    public void TierThree() {

    }

    public void TierTwo() {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
