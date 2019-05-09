using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Enums {
    public enum EnergyStates : int {
        ZERO_CHARGES,
        ONE_CHARGE,
        TWO_CHARGES,
        FULLY_CHARGED
    }

    public enum Limb : int {
        HEAD,
        ARMS,
        LEGS
    }
}