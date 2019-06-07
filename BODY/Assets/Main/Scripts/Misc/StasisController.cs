using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisController : MonoBehaviour
{
    private Rigidbody rigid;

    private Vector3 savedVelocity;
    public bool isPlatform;

    private void Start() {
        rigid = GetComponent<Rigidbody>();
    }

    public IEnumerator Stasis(float stasisTime) {
        savedVelocity = rigid.velocity;
        rigid.isKinematic = true;
        yield return new WaitForSecondsRealtime(stasisTime);
        rigid.isKinematic = false;
        rigid.velocity = savedVelocity;
    }
}