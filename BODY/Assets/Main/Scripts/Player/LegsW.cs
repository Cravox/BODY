using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsW : MonoBehaviour, IPlayerLimb
{
    public PlayerController playerCont { get { return PlayerController.instance; }}
    public bool fullyCharged { get { return (energyCount == EnergyState.Three);  }}
    public EnergyState energyCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Charge()
    {
        throw new System.NotImplementedException();
    }

    public void Discharge()
    {
        throw new System.NotImplementedException();
    }

    public void TierOne()
    {
        throw new System.NotImplementedException();
    }

    public void TierThree()
    {
        throw new System.NotImplementedException();
    }

    public void TierTwo()
    {
        throw new System.NotImplementedException();
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
