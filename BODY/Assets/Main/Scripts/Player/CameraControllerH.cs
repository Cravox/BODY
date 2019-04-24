using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraControllerH : SerializedMonoBehaviour
{
    [SerializeField, Required]
    private Transform target;

    private bool useOffsetValues;
    
    [SerializeField]
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if(!useOffsetValues) offset = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Horizontal");
        transform.LookAt(target);
        transform.position = target.position - offset;
    }
}