using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TriggerEnergypillar : TriggerContainer {
    private Material energyMat;

    [SerializeField]
    private Material[] energyMats;

    [SerializeField]
    private Color color;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gotActive) {
            transform.GetComponent<MeshRenderer>().materials[2].SetColor("_EmissionColor", color);
            transform.GetComponent<MeshRenderer>().materials[1] = energyMats[1];
        } else {
            transform.GetComponent<MeshRenderer>().materials[2].SetColor("_EmissionColor", Color.white);
            transform.GetComponent<MeshRenderer>().materials[1] = energyMats[0];

        }
    }
}
