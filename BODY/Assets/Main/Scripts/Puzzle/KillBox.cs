using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Carry")
            col.gameObject.GetComponent<CarryBox>().DestroyBox();
    }
}
