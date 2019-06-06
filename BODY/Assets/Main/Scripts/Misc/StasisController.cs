using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisController : MonoBehaviour
{
    public Rigidbody rigid;
    private Vector3 savedVelocity;


    public void StasisTrigger()
    {
        rigid.isKinematic = !rigid.isKinematic;
    }
}
