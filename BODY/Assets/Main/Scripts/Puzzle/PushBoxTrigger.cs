using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBoxTrigger : TriggerObject
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Push"))
        {
            triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Push"))
        {
            if(!triggerOnce)
                triggered = false;
        }
    }
}
