using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisController : MonoBehaviour
{
    public float StasisTime;

    private float stasisTimer = 0;

    public bool Triggered = false;

    public Rigidbody rigid;
    private Vector3 savedVelocity;
    public bool isPlatform;

    public IEnumerator Stasis(float stasisTime) {
        savedVelocity = rigid.velocity;
        rigid.isKinematic = true;
        yield return new WaitForSecondsRealtime(stasisTime);
        rigid.isKinematic = false;
        rigid.velocity = savedVelocity;
    }
}