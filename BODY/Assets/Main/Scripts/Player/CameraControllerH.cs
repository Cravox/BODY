using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraControllerH : SerializedMonoBehaviour
{
    [SerializeField, TabGroup("Settings")]
    private Vector3 offset;

    [SerializeField, Range(1, 10), TabGroup("Settings"), Tooltip("Defines how fast the camera rotates")]
    private float rotateSpeed;

    [SerializeField, TabGroup("Settings")]
    private bool useOffsetValues;

    [SerializeField, TabGroup("Settings")]
    private bool invertX;

    [SerializeField, TabGroup("Settings")]
    private bool invertY;

    [SerializeField, Required, TabGroup("References")]
    private Transform target;

    //[Required, TabGroup("References")]
    //public Transform pivot;
    
    private float horizontal;
    
    private float vertical;
   
    // Start is called before the first frame update
    void Start()
    {
        if(!useOffsetValues) offset = target.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {

    }
}