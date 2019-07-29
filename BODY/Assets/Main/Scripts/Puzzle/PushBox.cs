using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PushBox : SerializedMonoBehaviour {

    struct AngleVector {
        public float Angle;
        public Vector3 Direction;
    };

    private Vector3[] directions;

    [SerializeField]
    private bool pushed = false;

    [SerializeField]
    private float timeTreshold = 8;

    private Vector3 toPlayer;
    private Rigidbody rigid;

    public Vector3 moveDir;
    private RigidbodyConstraints constraints;

    [SerializeField]
    private float collisionTimer = 0;


    // Start is called before the first frame update
    void Start() {
        rigid = GetComponent<Rigidbody>();
        constraints = rigid.constraints;
        directions = new Vector3[] {
            transform.forward,
            -transform.forward,
            transform.right,
            -transform.right,
        };
    }

    private void Update() {
        if (pushed) {
            rigid.constraints = constraints;
        }

        if (rigid.velocity == Vector3.zero && !pushed) {
            rigid.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void FixedUpdate() {
        if (pushed) {
            rigid.velocity = moveDir;
        } else {
            rigid.velocity = Vector3.zero;
        }
    }

    public void PushedBox(Vector3 playerPosition, float pushForce) {
        collisionTimer = 0;
        pushed = true;
        toPlayer = playerPosition - transform.position;

        List<AngleVector> angleVectors = new List<AngleVector>(directions.Length);

        for (int i = 0; i < directions.Length; i++) {
            var angle = Vector3.Angle(directions[i], toPlayer);
            angleVectors.Add(
                new AngleVector() {
                    Angle = angle,
                    Direction = directions[i]
                }
            );
        }

        angleVectors.Sort((av1, av2) => av1.Angle.CompareTo(av2.Angle));

        moveDir = -angleVectors[0].Direction.normalized * pushForce;
    }

    private void OnCollisionEnter(Collision collision) {

    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag != "Player") {
            collisionTimer += Time.deltaTime;
            if (collisionTimer >= timeTreshold) {
                pushed = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag != "Player") {
            collisionTimer = 0;
        }
    }
}