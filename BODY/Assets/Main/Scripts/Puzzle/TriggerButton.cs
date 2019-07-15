using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButton : TriggerObject
{
    public Animator buttonAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(buttonAnim)
            buttonAnim.SetBool("Push", triggered);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Carry")
        {
            triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Carry")
        {
            if(!triggerOnce)
                triggered = false;
        }
    }
}