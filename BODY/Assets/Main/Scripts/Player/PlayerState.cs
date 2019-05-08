using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerLimb
{
    PlayerController playerCont { get; }
    bool fullyCharged { get; }
    EnergyState energyCount { get; set; }

    void TierOne();
    void TierTwo();

    void TierThree();

    void Charge();
    void Discharge();
}
public enum EnergyState
{
    One,
    Two,
    Three
}

public class PlayerState : MonoBehaviour
{
    List<IPlayerLimb> playerLimbs = new List<IPlayerLimb>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
    }
}
