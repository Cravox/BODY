using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerLimb {
    PlayerController playerCont { get; }
    bool FullyCharged { get; }
    EnergyStates EnergyState { get; }

    void TierOne();
    void TierTwo();
    void TierThree();

    void Charge();
    void Discharge();
}
