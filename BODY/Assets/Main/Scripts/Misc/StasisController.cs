using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisController : MonoBehaviour
{
    public Rigidbody rigid;
    private Vector3 savedVelocity;
    public bool isPlatform;

    public void StasisTrigger()
    {
        if (isPlatform)
        {
            MovingPlatform mp = GetComponent<MovingPlatform>();
            mp.stasis = !mp.stasis;
        }
        else
            rigid.isKinematic = !rigid.isKinematic;
    }
}
