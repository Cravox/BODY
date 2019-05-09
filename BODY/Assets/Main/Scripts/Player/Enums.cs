using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Enums {
    public enum EnergyStates : int {
        NOT_CHARGED,
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