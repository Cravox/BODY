using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerW : MonoBehaviour
{
    public float mouseSensitivity;
    public float camDistance;
    public float camFollowSpeed;
    public float camRotationSpeed;
    public Vector3 camRotation;
    public Transform rotationPivot;
    public Transform cam;
    public LayerMask nonPlayer;

    private Vector2 inputAxis;
    private float distanceToWall;
    void Update()
    {
        InputCheck();
        CamRotation();
        CamFollow();
        DistanceCheck();
    }

    void InputCheck()
    {
        inputAxis = Vector2.MoveTowards(inputAxis, new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")), 0.9f);
    }

    void DistanceCheck()
    {
        RaycastHit ray;

        if (Physics.Raycast(transform.position, -transform.forward, out ray, camDistance, nonPlayer))
            rotationPivot.localPosition = new Vector3(0, 0, -Vector3.Distance(transform.position, ray.point) + 0.5f);
        else
            rotationPivot.localPosition = new Vector3(0, 0, -camDistance);
    }

    //Change rotation by input axis from right stick
    void CamRotation()
    {
        camRotation += new Vector3(inputAxis.y, inputAxis.x, 0) * mouseSensitivity;
        camRotation = new Vector3(Mathf.Clamp(camRotation.x, -50, 80),camRotation.y, 0);

        transform.localEulerAngles = camRotation;
    }

    void CamFollow()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, rotationPivot.position, 6 * Time.deltaTime);
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, transform.rotation, 8 * Time.deltaTime);
    }
}
