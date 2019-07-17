using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StasisController : SerializedMonoBehaviour {

    //TO BE REMOVED


    //private Rigidbody rigid;
    //private MovingPlatform platform;

    //private Vector3 savedVelocity;
    //private RigidbodyConstraints constrains;
    //public bool isPlatform;


    //private void Start() {
    //    rigid = GetComponent<Rigidbody>();

    //    if (isPlatform)
    //        platform = GetComponent<MovingPlatform>();
    //}

    //public IEnumerator Stasis(float stasisTime) {
    //    savedVelocity = rigid.velocity;

    //    rigid.isKinematic = true;
    //    //constrains = rigid.constraints;
    //    //rigid.constraints = RigidbodyConstraints.FreezeAll;

    //    if (isPlatform)
    //        platform.stasis = true;

    //    yield return new WaitForSecondsRealtime(stasisTime);

    //    if (isPlatform)
    //        platform.stasis = false;
    //    else
    //        rigid.isKinematic = false;

    //    //rigid.constraints = constrains;

    //    rigid.velocity = savedVelocity;
    //}
}