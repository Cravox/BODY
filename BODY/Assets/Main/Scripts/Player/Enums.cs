using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Enums {
    public enum EnergyStates : int {
        ZERO_CHARGES,
        ONE_CHARGE,
        TWO_CHARGES,
        THREE_CHARGES,
        FOUR_CHARGES,
        FIVE_CHARGES,
        SIX_CHARGES,
        SEVEN_CHARGES,
        EIGHT_CHARGES,
        FULLY_CHARGED
    }

    public enum LimbIndex : int {
        HEAD,
        ARMS,
        LEGS
    }

    public enum ChargeState : int {
        NOT_CHARGED,
        TIER_ONE,
        TIER_TWO
    }
}