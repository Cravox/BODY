using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnAwake : MonoBehaviour
{
    public bool destroyObject;
    void Awake()
    {
        if (destroyObject)
            Destroy(this.gameObject);
        else
            gameObject.SetActive(false);
    }
}
