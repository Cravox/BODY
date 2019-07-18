using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class MovingPlatform : SerializedMonoBehaviour
{
    public Transform platform;
    public Rigidbody rigid;
    public MeshRenderer render;
    public LineRenderer line;
    public Material[] mats;

    public Transform currentPos { get { return positions[currentPosition]; } }

    public List<Transform> positions = new List<Transform>();
    public int currentPosition = 0;

    public float moveSpeed;

    public bool stop = true;
    public bool isActive { get { return !platCol.isTrigger; } }

    [HideInInspector]
    public Collider platCol;

    public static bool colliderEnabled;

    public bool stopForTurn;
    public bool turning;

    //Vector3 posTo;

    void Start()
    {
        platCol = platform.GetComponent<BoxCollider>();
        platCol.isTrigger = true;

        List<Vector3> posCoordinates = new List<Vector3>();

        for (int i = 0; i < positions.Count; i++)
        {
            posCoordinates.Add(positions[i].localPosition);
        }


        line.positionCount = posCoordinates.Count;
        line.SetPositions(posCoordinates.ToArray());
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
        //0 = green, 1 = red

        if (!platCol.isTrigger)
            render.material = mats[0];
        else 
            render.material = mats[1];
    }

    void Move()
    {
        if (platform.position != currentPos.position)
        {
            Vector3 newPos = Vector3.MoveTowards(platform.position, currentPos.position, moveSpeed * Time.fixedDeltaTime);
            rigid.MovePosition(newPos);
        }

        if (platform.position == currentPos.position)
                Next();
    }

    void Next()
    {
        if (!turning && currentPosition < positions.Count - 1)
            currentPosition++;
        else
            turning = true;

        if (turning && currentPosition == 0)
        {
            stop = true;
            platCol.isTrigger = true;
        }

        if (turning && currentPosition > 0)
            currentPosition--;
    }

    public void EnablePlatformCollider(bool enable) {
        platCol.enabled = enable;
    }
}