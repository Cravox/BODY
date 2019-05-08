using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum EnergyState : int {
    NOT_CHARGED,
    ONE_CHARGE,
    TWO_CHARGES,
    FULLY_CHARGED
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
