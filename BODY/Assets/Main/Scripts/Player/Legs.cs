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
    public bool wallJump { get { return Physics.Raycast(transform.position, playerCont.modelAxis.transform.forward, wJumpDistanceToWall, LayerMask.GetMask("wJump")); } }

    public override int TierOne() {
        if (playerCont.isGrounded) {
            playerCont.Jump();
            return tierCosts[0];
        } else {
            return 0;
        }
    }

    public override int TierTwo() {
        if (!playerCont.isGrounded && !doubleJumping && !wallJump) {
            doubleJumping = true;
            playerCont.modelAnim.Play("Dash");

            playerCont.rigid.velocity = new Vector3(playerCont.rigid.velocity.x, 0, playerCont.rigid.velocity.z);

            Vector3 dirForceSide = playerCont.modelAxis.forward * dJumpSpeed.z + playerCont.transform.right * dJumpSpeed.x;
            Vector3 dirForceUp = playerCont.transform.up * dJumpSpeed.y;

            playerCont.AddForce(dirForceUp, 0, out dJumpForce, true);
            playerCont.AddForce(dirForceSide, dJumpForwardTime, out dJumpForce, false);

            return tierCosts[1];
        }

        return 0;
    }

    public override int TierThree() {
        // walljump (atm)
        if (!playerCont.isGrounded && !wallJumping && wallJump) {
            wallJumping = true;
            playerCont.modelAnim.Play("Dash");

            playerCont.rigid.velocity = new Vector3(playerCont.rigid.velocity.x, 0, playerCont.rigid.velocity.z);

            Vector3 dirForceSide = playerCont.modelAxis.forward * wJumpSpeed.z + playerCont.transform.right * wJumpSpeed.x;
            Vector3 dirForceUp = playerCont.transform.up * wJumpSpeed.y;

            playerCont.AddForce(dirForceUp, 0, out wJumpForce, true);
            playerCont.AddForce(dirForceSide, wJumpForwardTime, out wJumpForce, false);
            return tierCosts[2];
        }

        Debug.DrawRay(transform.position, playerCont.modelAxis.transform.forward * wJumpDistanceToWall, Color.green, 0.1f);
        return 0;
        //playerCont.modelAnim.SetBool("IsDashing", wallJumping);


    }

    protected override void LimbStart() {

    }

    protected override void LimbUpdate() {
        playerCont.modelAnim.SetBool("IsDashing", doubleJumping);

        if (playerCont.isGrounded) //if grounded, cancel ongoing forces
        {
            if (doubleJumping && dJumpForce != null)
            { 
                dJumpForce.Stop();
                doubleJumping = false;
            }

            if (wallJumping && wJumpForce != null)
            {
                wJumpForce.Stop();
                wallJumping = false;
            }
        }
    }
}