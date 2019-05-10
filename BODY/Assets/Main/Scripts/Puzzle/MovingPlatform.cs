using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MovingPlatform : SerializedMonoBehaviour
{
    public Rigidbody rigid;
    public struct MovePos
    {
        public bool stay;
        public Transform pos;
        public float speed;
        public float time;
    }

    private List<Vector3> pos = new List<Vector3>();
    public List<MovePos> positions = new List<MovePos>();
    public int currentPosition = 1;
    public Vector3 distancePosition;
    private float timer = 0;

    void Start()
    {
        distancePosition = transform.position;

        if (positions != null)
        {
            foreach (MovePos p in positions)
            {
                pos.Add(p.pos.position);
            }
        }
    }

    void Update()
    {
        if (positions == null)
            return;

        TimerTick();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 posTo = pos[currentPosition];
        if (transform.position != posTo && !positions[currentPosition].stay)
        {
            float dist = Vector3.Distance(distancePosition, posTo);
            Vector3 newPos = Vector3.MoveTowards(transform.position, posTo, (dist / positions[currentPosition].time) * Time.fixedDeltaTime);
            rigid.MovePosition(newPos);
        }
    }

    void TimerTick()
    {
        timer += Time.deltaTime;

        if (timer >= positions[currentPosition].time)
        {
            Next();
            timer = 0;
        }
    }

    void Next()
    {
        distancePosition = transform.position;

        if (currentPosition >= positions.Count - 1)
            currentPosition = 0;
        else
            currentPosition++;
    }
}
