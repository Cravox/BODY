using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnAwake : MonoBehaviour
{
    public bool destroyObject;
    public Renderer renderDisable;

    void Awake()
    {
        if (destroyObject)
            Destroy(this.gameObject);
        else if (renderDisable)
            renderDisable.enabled = false;
        else
            gameObject.SetActive(false);
    }
}
