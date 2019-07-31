using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PushBox : SerializedMonoBehaviour {

    struct AngleVector {
        public float Angle;
        public Vector3 Direction;
    };

    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private bool pushed = false;

    [SerializeField]
    private Transform rayTrans;

    [SerializeField]
    private LayerMask layerMask;

    private Vector3[] directions;

    private Transform desiredTrans;

    private Vector3 toPlayer;
    private Vector3 aktPos;

    private float distance;

    private float lerpF = 0;


    // Start is called before the first frame update
    void Start() {
        //rigid = GetComponent<Rigidbody>();
        //constraints = rigid.constraints;
        directions = new Vector3[] {
            transform.forward,
            -transform.forward,
            transform.right,
            -transform.right,
        };
    }

    private void Update() {
        if (pushed && desiredTrans != null) {
            lerpF += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(aktPos, desiredTrans.position, lerpF/distance);
            if (lerpF/distance >= 1) {
                desiredTrans = null;
                pushed = false;
                lerpF = 0;
            }
        }
    }

    private void FixedUpdate() {

    }

    public void PushedBox(Vector3 playerPosition, float pushForce) {
        toPlayer = playerPosition - transform.position;
        aktPos = transform.position;
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

        //moveDir = -angleVectors[0].Direction.normalized * pushForce;
        Ray ray = new Ray();
        RaycastHit hit;
        ray.origin = rayTrans.position;
        ray.direction = -angleVectors[0].Direction.normalized;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100000, layerMask.value)) {
            if (hit.transform != null && hit.distance > 1.5f) {
                distance = hit.distance;
                pushed = true;
                desiredTrans = hit.transform;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Push")) {
            ResetBox();
        }
    }

    public void ResetBox() {
        pushed = false;
        desiredTrans = null;
        lerpF = 0;
    }
}