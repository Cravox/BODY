using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryBox : MonoBehaviour {
    private Rigidbody rigid;
    public CollisionField groundTrigger;

    public MovingPlatform platformOn;

    public Vector3 velocity;
    public bool gettingCarried;

    [HideInInspector]
    public Arms playerArms;

    // Start is called before the first frame update
    void Start() {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        if (transform.position.y < -50f) {
            DestroyBox();
        }

        //if (platformOn != null) {
        //    rigid.velocity += platformOn.rigid.velocity;
        //}

        if (groundTrigger.obj != null && groundTrigger.obj.gameObject.tag == "MovingPlatform") {
            platformOn = groundTrigger.obj.gameObject.GetComponent<MovingPlatform>();
        } else
            platformOn = null;

        velocity = rigid.velocity;
    }

    public void DestroyBox() {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("EnergyWall")) {
            if(playerArms != null)
            playerArms.DetachObject();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("EnergyWall")) {
            if(playerArms != null)
            playerArms.DetachObject();
            else {
                transform.parent = null;
                rigid.constraints = RigidbodyConstraints.None;
            }
        }
    }
}