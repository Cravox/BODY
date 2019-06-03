using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Legs : Limb {
    [TabGroup("Balancing"), Header("Double Jump"), Tooltip("X = Sideways, Y = Up, Z = Forward")]
    public Vector3 dJumpSpeed;
    [TabGroup("Balancing")]
    public float dJumpForwardTime = 0.5f;
    [TabGroup("Balancing")]
    public float wJumpDistanceToWall = 1f;

    [TabGroup("Balancing"), Header("Wall Jump"), Tooltip("X = Sideways, Y = Up, Z = Forward")]
    public Vector3 wJumpSpeed;
    [TabGroup("Balancing")]
    public float wJumpForwardTime = 0.5f;

    [TabGroup("Debugging")]
    public bool doubleJumping;
    [TabGroup("Debugging")]
    public PlayerForce dJumpForce;
    [TabGroup("Debugging")]
    public bool wallJumping;
    [TabGroup("Debugging")]
    public PlayerForce wJumpForce;
    [TabGroup("Debugging")]
    public bool wallJump;

    public override int TierOne() {
        if (playerCont.isGrounded) {
            playerCont.Jump();
            return tierCosts[0];
        } else {
            return 0;
        }
    }

    public override int TierTwo() {
        if (Input.GetButtonDown("Jump") && !playerCont.isGrounded && !doubleJumping && !wallJump) {
            doubleJumping = true;
            playerCont.modelAnim.Play("Dash");

            playerCont.rigid.velocity = new Vector3(playerCont.rigid.velocity.x, 0, playerCont.rigid.velocity.z);

            Vector3 dirForceSide = playerCont.modelAxis.forward * dJumpSpeed.z + playerCont.transform.right * dJumpSpeed.x;
            Vector3 dirForceUp = playerCont.transform.up * dJumpSpeed.y;

            playerCont.AddForce(dirForceUp, 0, out dJumpForce, true);
            playerCont.AddForce(dirForceSide, dJumpForwardTime, out dJumpForce, false);
        }

        if (playerCont.isGrounded && doubleJumping && dJumpForce != null) {
            dJumpForce.Stop();
            doubleJumping = false;
            return tierCosts[1];
        }

        playerCont.modelAnim.SetBool("IsDashing", doubleJumping);
        return 0;
    }

    public override int TierThree() {
        // walljump (atm)
        if (Input.GetButtonDown("Jump") && !playerCont.isGrounded && !wallJumping && wallJump) {
            wallJumping = true;
            playerCont.modelAnim.Play("Dash");

            playerCont.rigid.velocity = new Vector3(playerCont.rigid.velocity.x, 0, playerCont.rigid.velocity.z);

            Vector3 dirForceSide = playerCont.modelAxis.forward * wJumpSpeed.z + playerCont.transform.right * wJumpSpeed.x;
            Vector3 dirForceUp = playerCont.transform.up * wJumpSpeed.y;

            playerCont.AddForce(dirForceUp, 0, out wJumpForce, true);
            playerCont.AddForce(dirForceSide, wJumpForwardTime, out wJumpForce, false);
        }

        if (playerCont.isGrounded && wallJumping && wJumpForce != null) {
            wJumpForce.Stop();
            wallJumping = false;
        }

        Debug.DrawRay(transform.position, playerCont.modelAxis.transform.forward * wJumpDistanceToWall, Color.green, 0.1f);
        return 0;
        //playerCont.modelAnim.SetBool("IsDashing", wallJumping);


    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {
        //bool canWallJump = (EnergyState == Enums.EnergyStates.FULLY_CHARGED);
        bool ray = Physics.Raycast(transform.position, playerCont.modelAxis.transform.forward, wJumpDistanceToWall, LayerMask.GetMask("wJump"));

        Debug.Log(ray);

        //wallJump = (canWallJump && ray);
    }
}