using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Legs : Limb {
    [TabGroup("Balancing"), Header("Double Jump"), Tooltip("X = Sideways, Y = Up, Z = Forward")]
    public Vector3 dJumpSpeed;

    [TabGroup("Balancing"), Header("Wall Jump"), Tooltip("X = Sideways, Y = Up, Z = Forward")]
    public Vector3 wJumpSpeed;
    [TabGroup("Balancing")]
    public float wJumpDistanceToWall = 1f;
    [TabGroup("Balancing"), Header("Hover")]
    public float timeToHover;

    [TabGroup("References")]
    public LayerMask rMask;

    [TabGroup("Debugging")]
    public bool jumping;
    [TabGroup("Debugging")]
    public bool doubleJumping;
    [TabGroup("Debugging")]
    public bool wallJumping;

    [TabGroup("Debugging")]
    public Transform wallRay;
    [TabGroup("Debugging")]
    public bool hover;

    private bool hasHovered;
    private Coroutine hoverCoroutine;

    private bool wallJump { get { return Physics.OverlapSphere(wallRay.position, wJumpDistanceToWall, rMask).Length > 0; } }

    public override void BaselineAbility() {
        if (playerCont.isGrounded && !jumping && !hover) {
            Vector3 jumpSpeed = playerCont.moveForce * (playerCont.walkSpeed - playerCont.airSpeed) + new Vector3(0, playerCont.jumpSpeed, 0);
            playerCont.JumpOnce(jumpSpeed, true, 0);
            jumping = true;
        }
    }

    public override int TierOne() {
        if (!playerCont.isGrounded && !doubleJumping && !hover) {
            playerCont.JumpOnce(dJumpSpeed, false, 1);
            doubleJumping = true;
            return tierCosts[0];
        }

        return 0;
    }

    public override int TierTwo() {
        if (!playerCont.isGrounded && !hasHovered) {
            playerCont.StopAllForces();
            playerCont.rigid.velocity = Vector3.zero;
            hoverCoroutine = StartCoroutine(hoverTime(timeToHover));
            hasHovered = true;
            return tierCosts[1];
        } else
            StopHover();

        return 0;
        //if (!playerCont.isGrounded && wallJump && !hover) {
        //    playerCont.StopAllForces(); //stop all forces to make walljump clean
        //    playerCont.JumpOnce(wJumpSpeed, false, 2);
        //    wallJumping = true;
        //    return tierCosts[1];
        //}
        //return 0;
    }

    public override int TierThree() {
        // hover

        if (!playerCont.isGrounded && !hasHovered) {
            playerCont.StopAllForces();
            playerCont.rigid.velocity = Vector3.zero;
            hoverCoroutine = StartCoroutine(hoverTime(timeToHover));
            hasHovered = true;
        } else
            StopHover();

        return 0;

        //playerCont.modelAnim.SetBool("IsDashing", wallJumping);
    }

    void StopHover() {
        StopCoroutine(hoverCoroutine);
        hover = false;
    }

    IEnumerator hoverTime(float t) {
        hover = true;
        yield return new WaitForSeconds(t);
        hover = false;
    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {
        playerCont.modelAnim.SetBool("IsDashing", doubleJumping);

        if (playerCont.isGrounded) //if grounded, cancel ongoing forces
        {
            jumping = false;
            doubleJumping = false;
            wallJumping = false;

            if (hoverCoroutine != null)
                StopHover();

            hasHovered = false;
        }

        playerCont.hovering = hover;

        playerCont.rigid.constraints =
            (hover ? RigidbodyConstraints.FreezePositionY : RigidbodyConstraints.None)
            | RigidbodyConstraints.FreezeRotationX
            | RigidbodyConstraints.FreezeRotationY
            | RigidbodyConstraints.FreezeRotationZ;
    }

    protected override void UpdateLimbUI() {
        switch (chargeState) {
            case Enums.ChargeState.NOT_CHARGED:
                if (playerCont.isGrounded)
                    limbText.text = "Jump";
                else
                    limbText.text = "";
                break;
            case Enums.ChargeState.TIER_ONE:
                if (!playerCont.isGrounded && !doubleJumping)
                    limbText.text = "Double Jump";
                else
                    limbText.text = "";
                break;
            case Enums.ChargeState.TIER_TWO:
                if (!playerCont.isGrounded && hover)
                    limbText.text = "End Hover";
                else if (!playerCont.isGrounded && !hover && !hasHovered)
                    limbText.text = "Start Hover";
                else
                    limbText.text = "";
                break;
            default:
                break;
        }
    }
}