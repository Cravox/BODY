using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab;
    public GameObject currentBox;

    void Update()
    {
        if (currentBox == null)
            SpawnBox();
    }

    public void Reset()
    {
        currentBox.GetComponent<CarryBox>().DestroyBox();
    }

    void SpawnBox()
    {
        currentBox = Instantiate(boxPrefab, transform.position, Quaternion.identity) as GameObject;
    }
}
