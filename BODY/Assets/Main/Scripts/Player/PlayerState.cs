using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerLimb
{
    PlayerController playerCont { get; set; }
    bool fullyCharged { get; set; }
    int energyCount { get; set; }

    void TierOne();
    void TierTwo();

    void TierThree();

    void Charge();
    void Discharge();
    void Update();
    void FixedUpdate();
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
        foreach(IPlayerLimb pl in playerLimbs)
        {
            pl.Update();
        }
    }

    void FixedUpdate()
    {
        foreach (IPlayerLimb pl in playerLimbs)
        {
            pl.FixedUpdate();
        }
    }
}
