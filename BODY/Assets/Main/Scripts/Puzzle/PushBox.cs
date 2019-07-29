﻿using System.Collections;
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

    private Vector3 toPlayer;
    private Rigidbody rigid;

    public Vector3 moveDir;
    private RigidbodyConstraints constraints;


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
        } else {
            rigid.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void FixedUpdate() {

        rigid.velocity = moveDir;
    }

    public void PushedBox(Vector3 playerPosition, float pushForce) {
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
        if (collision.gameObject.CompareTag("Push")) {
            moveDir = Vector3.zero;
            pushed = false;
        }
    }
}