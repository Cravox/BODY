using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimEvents : MonoBehaviour {
    [SerializeField]
    private ParticleSystem[] jumpThrusterVFXs;

    [SerializeField]
    private GameObject[] hoverVFXs;

    public GameObject pushImpulseVFX;

    public GameObject vfxLeftToes;
    public GameObject vfxLeftFoot;
    public GameObject vfxRightToes;
    public GameObject vfxRightFoot;

    public Transform[] handTransforms = new Transform[2];
    public GameObject vfxLeftHand;
    public GameObject vfxRightHand;

    public GameObject vfxHover1;
    public GameObject vfxHover2;
    public GameObject vfxHover3;
    public GameObject vfxHover4;
    public GameObject vfxHover5;
    public GameObject vfxHover6;
    public GameObject vfxHover7;
    public GameObject vfxHover8;

    public GameObject vfxShoulderLeft;
    public GameObject vfxShoulderRight;
    public GameObject vfxArmLeft;
    public GameObject vfxArmRight;
    public GameObject vfxLegLeft;
    public GameObject vfxLegRight;

    public GameObject vfxLightLeftHand;
    public GameObject vfxLightRightHand;

    public GameObject vfxLightLeftFoot;
    public GameObject vfxLightRightFoot;

    public GameObject vfxCarry;
    public GameObject vfxCarryBeamA;
    public GameObject vfxCarryBeamB;
    public GameObject vfxReceive;
    public GameObject vfxThrow;
    public GameObject vfxActivation;

    public GameObject charRoot;

    private bool isDoubleJumping;
    public bool isPushing;

    private Animator anim;

    [SerializeField]
    private Arms arms;

    void Start() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        vfxCarryBeamA.transform.LookAt(vfxReceive.transform);
        vfxCarryBeamB.transform.LookAt(vfxReceive.transform);

        if (anim.GetBool("isHovering") == true) {
            foreach (GameObject vfx in hoverVFXs) {
                vfx.SetActive(true);
            }

            vfxLightLeftFoot.SetActive(true);
            vfxLightRightFoot.SetActive(true);
        } else {
            foreach (GameObject vfx in hoverVFXs) {
                vfx.SetActive(false);
            }

            if (isDoubleJumping == false) {
                vfxLightLeftFoot.SetActive(false);
                vfxLightRightFoot.SetActive(false);
            }
        }

        if (anim.GetBool("isFullPower") == true) {
            vfxShoulderLeft.SetActive(true);
            vfxShoulderRight.SetActive(true);
            vfxArmLeft.SetActive(true);
            vfxArmRight.SetActive(true);
            vfxLegLeft.SetActive(true);
            vfxLegRight.SetActive(true);
        } else {
            vfxShoulderLeft.SetActive(false);
            vfxShoulderRight.SetActive(false);
            vfxArmLeft.SetActive(false);
            vfxArmRight.SetActive(false);
            vfxLegLeft.SetActive(false);
            vfxLegRight.SetActive(false);
        }
    }

    void JumpThrusters() {
        isDoubleJumping = true;

        vfxLightLeftFoot.SetActive(true);
        vfxLightRightFoot.SetActive(true);

        foreach (ParticleSystem vfx in jumpThrusterVFXs) {
            vfx.Play();
        }
    }

    void PushImpulse() {
        isPushing = true;

        vfxLightLeftHand.SetActive(true);
        vfxLightRightHand.SetActive(true);

        for (int i = 0; i < handTransforms.Length; i++) {
            var vfx = Instantiate(pushImpulseVFX, handTransforms[i].position, Quaternion.identity);
            vfx.transform.localEulerAngles = charRoot.transform.localEulerAngles;
        }

        arms.PushBox();
    }

    void PickUp() {
        vfxCarry.SetActive(true);
        vfxReceive.SetActive(true);
        vfxLightRightHand.SetActive(true);
    }

    void Drop() {
        if (isPushing == false) {
            vfxCarry.SetActive(false);
            vfxReceive.SetActive(false);
            vfxLightRightHand.SetActive(false);
        }
    }

    void ThrowImpulse() {
        vfxCarry.SetActive(false);
        vfxReceive.SetActive(false);

        GameObject pushImpulseObj1 = Instantiate(pushImpulseVFX);   //Spawn vFX

        pushImpulseObj1.transform.parent = charRoot.transform;      // Parent to root to get Character oriantation

        pushImpulseObj1.transform.localRotation = Quaternion.Euler(90, 0, 0);       // set Rotation relative to character

        pushImpulseObj1.transform.parent = vfxRightHand.transform;       // Parent to VFX object

        pushImpulseObj1.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);    // Get VFX object location

        pushImpulseObj1.transform.parent = charRoot.transform;          // Unparent from VFX to avoid weird movment

        if (isPushing == false) {
            vfxLightRightHand.SetActive(false);
        }
    }

    void DoubleJumpEnd() {
        isDoubleJumping = false;

        if (anim.GetBool("isHovering") == false) {
            vfxLightLeftFoot.SetActive(false);
            vfxLightRightFoot.SetActive(false);
        }
    }

    void PushEnd() {
        isPushing = false;

        if (anim.GetBool("isCarrying") == false) {
            vfxLightLeftHand.SetActive(false);
            vfxLightRightHand.SetActive(false);
        }
        GameManager.instance.CanControl = true;
    }
}
