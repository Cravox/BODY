using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class MovingPlatform : SerializedMonoBehaviour
{
    [TabGroup("Balancing")]
    public float moveSpeed;

    [TabGroup("References")]
    public Transform platform;
    [TabGroup("References")]
    public Rigidbody rigid;
    [TabGroup("References")]
    public Animator animate;
    [TabGroup("References")]
    public LineRenderer line;

    private Transform currentPos { get { return positions[currentPosition]; } }
    private int currentPosition = 0;

    [TabGroup("Debugging")]
    public bool stop = true;
    public bool isActive { get { return !platCol.isTrigger; } }

    [HideInInspector]
    public Collider platCol;

    [TabGroup("Debugging")]
    public bool turning;

    [Header("Positions"), InfoBox("Please keep the first position point on Root position.")]
    public List<Transform> positions = new List<Transform>();


    [Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
    private void UpdateLinesInEditor()
    {
        UpdateLineRenderer();
    }

    //Vector3 posTo;

    void Start()
    {
        platCol = platform.GetComponent<BoxCollider>();
        platCol.isTrigger = true;

        UpdateLineRenderer();
    }

    void Update()
    {
        if (positions == null)
            return;

        Feedback();
    }

    void FixedUpdate()
    {
        if (positions == null || stop)
            return;

        Move();
    }

    void Feedback()
    {
        //0 = green, 1 = red

        //animate.SetBool("Triggered", !platCol.isTrigger);
        //render.enabled = !platCol.isTrigger;
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
            animate.SetTrigger("Triggered");
            platCol.isTrigger = true;
            turning = false;
        }

        if (turning && currentPosition > 0)
            currentPosition--;
    }

    public void EnablePlatformCollider(bool enable) {
        platCol.enabled = enable;
    }

    void UpdateLineRenderer()
    {
        List<Vector3> posCoordinates = new List<Vector3>();

        for (int i = 0; i < positions.Count; i++)
        {
            posCoordinates.Add(positions[i].localPosition);
        }


        line.positionCount = posCoordinates.Count;
        line.SetPositions(posCoordinates.ToArray());
    }
}