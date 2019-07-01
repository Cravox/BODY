using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryBox : MonoBehaviour {
    [SerializeField]
    private bool isTrigger;
    private Rigidbody rigid;
    public CollisionField groundTrigger;

    [HideInInspector]
    public bool FirstPickUp = false;
    private bool pickedUp = false;
    public MovingPlatform platformOn;

    [SerializeField]
    private List<Door> triggeredObjects;

    public Vector3 velocity;

    // Start is called before the first frame update
    void Start() {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (FirstPickUp && isTrigger && !pickedUp) {
            foreach (var door in triggeredObjects) {
                door.Open(true);
                pickedUp = true;
            }
        }

        if(platformOn != null)
            rigid.velocity = platformOn.rigid.velocity;

        if (groundTrigger.obj != null && groundTrigger.obj.gameObject.tag == "MovingPlatform")
        {
            platformOn = groundTrigger.obj.gameObject.GetComponent<MovingPlatform>();
        }
        else
            platformOn = null;


        velocity = rigid.velocity;
    }
}