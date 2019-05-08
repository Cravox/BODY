using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerLimb {
    PlayerController playerCont { get; }
    bool fullyCharged { get; set; }
    EnergyState energyState { get; set; }

    void TierOne();
    void TierTwo();
    void TierThree();

    void Charge();
    void Discharge();
}
