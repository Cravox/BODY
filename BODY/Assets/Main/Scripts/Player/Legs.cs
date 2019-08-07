using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Legs : Limb {
    [SerializeField, TabGroup("Balancing"), Header("Double Jump"), Tooltip("X = Sideways, Y = Up, Z = Forward")]
    private Vector3 dJumpSpeed;

    [SerializeField, TabGroup("Balancing"), Header("Hover")]
    private float timeToHover;

    [SerializeField, TabGroup("Debugging")]
    private bool jumping;

    [SerializeField, TabGroup("Debugging")]
    private bool doubleJumping;

    [SerializeField, TabGroup("Debugging")]
    private bool wallJumping;

    [SerializeField, TabGroup("Debugging")]
    private Transform wallRay;

    [SerializeField, TabGroup("Debugging")]
    private bool hover;

    private bool hasHovered;
    private Coroutine hoverCoroutine;

    public override void BaselineAbility() {
        if (playerCont.isGrounded && !jumping && !hover) {
            Vector3 jumpSpeed = playerCont.moveForce * (playerCont.walkSpeed - playerCont.airSpeed) + new Vector3(0, playerCont.jumpSpeed, 0);
            playerCont.JumpOnce(jumpSpeed, true, 0);
            jumping = true;
            playerCont.modelAnim.SetTrigger("Jump");
            SoundController.Play(gameObject, SoundController.Sounds.CHAR_JUMP, 128, 0.5f);
        }
    }

    public override int TierOne() {
        if (!playerCont.isGrounded && !doubleJumping && !hover) {
            playerCont.JumpOnce(dJumpSpeed, false, 1);
            doubleJumping = true;
            playerCont.modelAnim.SetTrigger("DoubleJump");
            SoundController.Play(gameObject, SoundController.Sounds.CHAR_DOUBLEJUMP, 128, 0.5f);
            return tierCosts[0];
        }

        return 0;
    }

    public override int TierTwo() {
        if (!playerCont.isGrounded && !hasHovered) {
            playerCont.StopAllForces();
            playerCont.rigid.velocity = Vector3.zero;
            hoverCoroutine = StartCoroutine(HoverTime(timeToHover));
            hasHovered = true;
            playerCont.modelAnim.SetBool("isHovering", true);
            playerCont.modelAnim.SetBool("isFullPower", true);
            SoundController.Play(gameObject, SoundController.Sounds.CHAR_HOVER, 128, 0.5f);
            return tierCosts[1];
        } else
            StopHover();

        return 0;
    }

    public override int TierThree() {
        if (!playerCont.isGrounded && !hasHovered) {
            playerCont.StopAllForces();
            playerCont.rigid.velocity = Vector3.zero;
            hoverCoroutine = StartCoroutine(HoverTime(timeToHover));
            hasHovered = true;
        } else
            StopHover();

        return 0;
    }

    void StopHover() {
        if (hoverCoroutine != null)
            StopCoroutine(hoverCoroutine);
        playerCont.modelAnim.SetBool("isHovering", false);
        playerCont.modelAnim.SetBool("isFullPower", false);
        hover = false;
    }

    IEnumerator HoverTime(float t) {
        hover = true;
        yield return new WaitForSeconds(t);
        playerCont.modelAnim.SetBool("isFullPower", false);
        hover = false;
    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {
        if (playerCont.isGrounded) //if grounded, cancel ongoing forces
        {
            jumping = false;
            doubleJumping = false;
            wallJumping = false;

            if (hoverCoroutine != null) {
                StopHover();
            }

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
        if (playerCont.isGrounded && Input.GetAxis("LeftTrigger") >= 0.9f) {
            limbImage.sprite = actionSprite[1];
        } else if (playerCont.isGrounded) {
            limbImage.sprite = actionSprite[0];
        }

        if(!playerCont.isGrounded && Input.GetAxis("LeftTrigger") >= 0.9f) {
            limbImage.sprite = actionSprite[3];
        } else if(!playerCont.isGrounded && Input.GetAxis("LeftTrigger") >= 0.9f && hover) {
            limbImage.sprite = actionSprite[4];
        } else if (!playerCont.isGrounded && !doubleJumping) {
            limbImage.sprite = actionSprite[2];
        }
    }

    public override void InputCheck() {
        if (Input.GetButtonDown("Jump") && (Input.GetAxis("LeftTrigger") >= 0.9f)) {
            if (!playerCont.isGrounded) {
                TierTwo();
            } else {
                BaselineAbility();
            }
        } else if (Input.GetButtonDown("Jump")) {
            if (playerCont.isGrounded) {
                BaselineAbility();
            } else {
                TierOne();
            }
        }
    }
}