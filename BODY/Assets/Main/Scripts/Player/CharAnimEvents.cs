using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimEvents : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] jumpThrusterVFXs = new ParticleSystem[8];

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

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        vfxCarryBeamA.transform.LookAt(vfxReceive.transform);
        vfxCarryBeamB.transform.LookAt(vfxReceive.transform);

        if (anim.GetBool("isHovering") == true)
        {
            vfxHover1.SetActive(true);
            vfxHover2.SetActive(true);
            vfxHover3.SetActive(true);
            vfxHover4.SetActive(true);
            vfxHover5.SetActive(true);
            vfxHover6.SetActive(true);
            vfxHover7.SetActive(true);
            vfxHover8.SetActive(true);

            vfxLightLeftFoot.SetActive(true);
            vfxLightRightFoot.SetActive(true);
        }

        else
        {
            vfxHover1.SetActive(false);
            vfxHover2.SetActive(false);
            vfxHover3.SetActive(false);
            vfxHover4.SetActive(false);
            vfxHover5.SetActive(false);
            vfxHover6.SetActive(false);
            vfxHover7.SetActive(false);
            vfxHover8.SetActive(false);

            if (isDoubleJumping == false)
            {
                vfxLightLeftFoot.SetActive(false);
                vfxLightRightFoot.SetActive(false);
            }
        }

        if (anim.GetBool("isFullPower") == true)
        {
            vfxShoulderLeft.SetActive(true);
            vfxShoulderRight.SetActive(true);
            vfxArmLeft.SetActive(true);
            vfxArmRight.SetActive(true);
            vfxLegLeft.SetActive(true);
            vfxLegRight.SetActive(true);
        }

        else
        {
            vfxShoulderLeft.SetActive(false);
            vfxShoulderRight.SetActive(false);
            vfxArmLeft.SetActive(false);
            vfxArmRight.SetActive(false);
            vfxLegLeft.SetActive(false);
            vfxLegRight.SetActive(false);
        }
    }

    void jumpThrusters()
    {
        isDoubleJumping = true;

        vfxLightLeftFoot.SetActive(true);
        vfxLightRightFoot.SetActive(true);

        foreach (ParticleSystem vfx in jumpThrusterVFXs) {
            vfx.Play();
        }

        //GameObject jumpThrustersObj1 = Instantiate(jumpThrustersVFX);   //Spawn VFX
        //GameObject jumpThrustersObj2 = Instantiate(jumpThrustersVFX);
        //GameObject jumpThrustersObj3 = Instantiate(jumpThrustersVFX);
        //GameObject jumpThrustersObj4 = Instantiate(jumpThrustersVFX);
        //
        //GameObject jumpThrustersObj5 = Instantiate(jumpThrustersVFX);
        //GameObject jumpThrustersObj6 = Instantiate(jumpThrustersVFX);
        //GameObject jumpThrustersObj7 = Instantiate(jumpThrustersVFX);
        //GameObject jumpThrustersObj8 = Instantiate(jumpThrustersVFX);
        //
        //jumpThrustersObj1.transform.parent = vfxLeftToes.transform;     //Parent to VFX
        //jumpThrustersObj2.transform.parent = vfxLeftToes.transform;
        //jumpThrustersObj3.transform.parent = vfxLeftToes.transform;
        //jumpThrustersObj4.transform.parent = vfxLeftFoot.transform;
        //
        //jumpThrustersObj1.transform.localPosition = new Vector3(0.0f, -0.03f, 0.0f);        //Set Rotation and Location relative to parent
        //jumpThrustersObj1.transform.localRotation = Quaternion.Euler(90, 0, 0);
        //jumpThrustersObj2.transform.localPosition = new Vector3(-0.02f, -0.03f, -0.03f);
        //jumpThrustersObj2.transform.localRotation = Quaternion.Euler(90, 0, 0);
        //jumpThrustersObj3.transform.localPosition = new Vector3(0.015f, -0.03f, -0.03f);
        //jumpThrustersObj3.transform.localRotation = Quaternion.Euler(90, 0, 0);
        //jumpThrustersObj4.transform.localPosition = new Vector3(0.0f, -0.08f, 0.0f);
        //jumpThrustersObj4.transform.localRotation = Quaternion.Euler(90, 0, 0);
        //
        //jumpThrustersObj5.transform.parent = vfxRightToes.transform;
        //jumpThrustersObj6.transform.parent = vfxRightToes.transform;
        //jumpThrustersObj7.transform.parent = vfxRightToes.transform;
        //jumpThrustersObj8.transform.parent = vfxRightFoot.transform;
        //
        //jumpThrustersObj5.transform.localPosition = new Vector3(0.0f, -0.03f, 0.0f);
        //jumpThrustersObj5.transform.localRotation = Quaternion.Euler(90, 0, 0);
        //jumpThrustersObj6.transform.localPosition = new Vector3(0.02f, -0.03f, -0.03f);
        //jumpThrustersObj6.transform.localRotation = Quaternion.Euler(90, 0, 0);
        //jumpThrustersObj7.transform.localPosition = new Vector3(-0.015f, -0.03f, -0.03f);
        //jumpThrustersObj7.transform.localRotation = Quaternion.Euler(90, 0, 0);
        //jumpThrustersObj8.transform.localPosition = new Vector3(0.0f, -0.08f, 0.0f);
        //jumpThrustersObj8.transform.localRotation = Quaternion.Euler(90, 0, 0);        
    }

    void pushImpulse()
    {
        isPushing = true;

        vfxLightLeftHand.SetActive(true);
        vfxLightRightHand.SetActive(true);

        //GameObject pushImpulseObj1 = Instantiate(pushImpulseVFX);   //Spawn vFX
        //GameObject pushImpulseObj2 = Instantiate(pushImpulseVFX);

        for (int i = 0; i < handTransforms.Length; i++) {
            var vfx = Instantiate(pushImpulseVFX, handTransforms[i].position, Quaternion.identity);
            vfx.transform.localEulerAngles = charRoot.transform.localEulerAngles;
        }

        arms.MoveBox();

        //pushImpulseObj1.transform.parent = charRoot.transform;      // Parent to root to get Character oriantation
        //pushImpulseObj2.transform.parent = charRoot.transform;
        //
        //pushImpulseObj1.transform.localRotation = Quaternion.Euler(90, 0, 0);       // set Rotation relative to character
        //pushImpulseObj2.transform.localRotation = Quaternion.Euler(90, 0, 0);
        //
        //pushImpulseObj1.transform.parent = vfxLeftHand.transform;       // Parent to VFX object
        //pushImpulseObj2.transform.parent = vfxRightHand.transform;
        //
        //pushImpulseObj1.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);    // Get VFX object location
        //pushImpulseObj2.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        //
        //pushImpulseObj1.transform.parent = charRoot.transform;          // Unparent from VFX to avoid weird movment
        //pushImpulseObj2.transform.parent = charRoot.transform;        
    }

    void pickUp()
    {
        vfxCarry.SetActive(true);
        vfxReceive.SetActive(true);
        vfxLightRightHand.SetActive(true);
    }

    void drop()
    {
        if (isPushing == false)
        {
            vfxCarry.SetActive(false);
            vfxReceive.SetActive(false);
            vfxLightRightHand.SetActive(false);
        }
    }

    void throwImpulse()
    {
        vfxCarry.SetActive(false);
        vfxReceive.SetActive(false);

        GameObject pushImpulseObj1 = Instantiate(pushImpulseVFX);   //Spawn vFX

        pushImpulseObj1.transform.parent = charRoot.transform;      // Parent to root to get Character oriantation

        pushImpulseObj1.transform.localRotation = Quaternion.Euler(90, 0, 0);       // set Rotation relative to character

        pushImpulseObj1.transform.parent = vfxRightHand.transform;       // Parent to VFX object

        pushImpulseObj1.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);    // Get VFX object location

        pushImpulseObj1.transform.parent = charRoot.transform;          // Unparent from VFX to avoid weird movment

        if (isPushing == false)
        {
            vfxLightRightHand.SetActive(false);
        }
    }

    void doubleJumpEnd()
    {
        isDoubleJumping = false;

        if (anim.GetBool("isHovering") == false)
        {
            vfxLightLeftFoot.SetActive(false);
            vfxLightRightFoot.SetActive(false);
        }
    }

    void PushEnd()
    {
        isPushing = false;

        if (anim.GetBool("isCarrying") == false)
        {
            vfxLightLeftHand.SetActive(false);
            vfxLightRightHand.SetActive(false);
        }
    }
}
