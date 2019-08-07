using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TriggerEnergypillar : TriggerContainer {
    private Material energyMat;

    private MeshRenderer ren;

    [SerializeField]
    private Material[] energyMats;

    [SerializeField]
    private Color color;

    // Start is called before the first frame update
    void Start() {
        ren = transform.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update() {
        if (gotActive) {
            ren.materials[2].SetColor("_EmissionColor", color);
            ren.materials[1] = energyMats[1];
        } else {
            ren.materials[2].SetColor("_EmissionColor", Color.white);
            ren.materials[1] = energyMats[0];

        }
    }
}
