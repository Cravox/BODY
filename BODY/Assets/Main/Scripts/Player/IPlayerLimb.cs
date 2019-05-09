using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerLimb {
    Enums.Limb Limb { get; }
    PlayerController playerCont { get; }
    bool FullyCharged { get; }
    Enums.EnergyStates EnergyState { get; }

    void TierOne();
    void TierTwo();
    void TierThree();

    void Charge();
    void Discharge();
}
