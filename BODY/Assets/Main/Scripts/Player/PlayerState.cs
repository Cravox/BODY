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
public enum EnergyState
{
    One = 0,
    Two = 1,
    Three = 2
}

public class PlayerState : SerializedMonoBehaviour
{
    [SerializeField]
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
