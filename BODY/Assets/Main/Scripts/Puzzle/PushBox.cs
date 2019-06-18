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

    private Vector3 toPlayer;
    
    // Start is called before the first frame update
    void Start() {
        directions = new Vector3[] {
            transform.forward,
            -transform.forward,
            transform.right,
            -transform.right,
        };
    }

    public void PushedBox(Vector3 playerPosition, float pushForce) {
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
        GetComponent<Rigidbody>().AddForce(-angleVectors[0].Direction.normalized * pushForce, ForceMode.Impulse);
    }
}