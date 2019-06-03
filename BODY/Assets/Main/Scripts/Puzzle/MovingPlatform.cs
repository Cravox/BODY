using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class MovePos
{
    public Transform obj;
    public Vector3 pos;
    public float time;
    public bool stay;

    public void SetPos()
    {
        this.pos = this.obj.position;
    }

    public void SetPos(Vector3 vec)
    {
        this.pos = vec;
    }
}

public class MovingPlatform : SerializedMonoBehaviour
{
    public Rigidbody rigid;
    public MeshRenderer renderer;
    public Material[] mats;

    public MovePos currentPos;
    public Vector3 distancePosition;

    public List<MovePos> positions = new List<MovePos>();
    public int currentPosition = 0;

    public MovePos dirChangePos;
    public static bool dirChangeActive = false;

    public static bool stop = true;

    [HideInInspector]
    public Collider platCol;

    public static bool colliderEnabled;

    public bool canChangeDirection { get { return (dirChangePos.obj != null); } }

    bool upwards;
    bool waitNext;
    //Vector3 posTo;

    void Start()
    {
        if (positions != null)
        {
            foreach (MovePos p in positions)
            {
                p.SetPos();
            }

            dirChangePos.pos = dirChangePos.obj.position;

            Next();
        }
        platCol = GetComponent<BoxCollider>();
        platCol.enabled = false;
    }

    void Update()
    {
        if (positions == null)
            return;

        ColorsFeedback();
    }

    void FixedUpdate()
    {
        if (positions == null || stop)
            return;

        Move();
    }

    void ColorsFeedback()
    {
        //0 = green, 1 = yellow, 2 = red

        if (canChangeDirection && dirChangeActive)
        {
            if (upwards)
                renderer.material = mats[2];
            else
                renderer.material = mats[1];
        }
        else if(platCol.enabled)
            renderer.material = mats[0];
        else if (!platCol.enabled) {
            renderer.material = mats[3];
        }
    }

    void Move()
    {
        if (transform.position != currentPos.pos && !currentPos.stay)
        {
            float dist = Vector3.Distance(distancePosition, currentPos.pos);
            Vector3 newPos = Vector3.MoveTowards(transform.position, currentPos.pos, (dist / currentPos.time) * Time.fixedDeltaTime);
            rigid.MovePosition(newPos);
        }

        if (transform.position == currentPos.pos)
        {
            upwards = (canChangeDirection && dirChangeActive);

            if (upwards)
                NextUp();
            else
                Next();
        }
    }

    void Next()
    {
        distancePosition = transform.position;

        if (!waitNext)
        {
            if (currentPosition >= positions.Count - 1)
                currentPosition = 0;
            else
                currentPosition++;
        }
        else
            waitNext = false;

        currentPos = positions[currentPosition];
    }

    void NextUp()
    {
        distancePosition = transform.position;
        dirChangePos.SetPos(new Vector3(positions[currentPosition].pos.x, dirChangePos.pos.y, positions[currentPosition].pos.z));

        ///////////////////////////////////////////////
        if (transform.position == dirChangePos.pos)
        {
            currentPos = positions[currentPosition];
            waitNext = false;
        }
        else
        {
            waitNext = true;
            currentPos = dirChangePos;
        }
        ///////////////////////////////////////////////
    }

    public void EnablePlatformCollider(bool enable) {
        platCol.enabled = enable;
    }
}