using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPuddleState : PlayerState
{
    protected int xInput;
    protected int yInput;

    private bool jumpInput;
    private bool isGrounded;

    public PlayerPuddleState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        player.puddleParticle.Play();
    }

    public override void Exit()
    {
        base.Exit();

        player.puddleParticle.Stop();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormalizedInputX;
        yInput = player.InputHandler.NormalizedInputY;
        jumpInput = player.InputHandler.JumpInput;

        player.CheckIfShouldFlip(xInput);

        if (jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (yInput > 0)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (xInput == 0)
        {
            player.RB.AddForce(new Vector2(playerData.movementVelocity * (player.CurrentVelocity.x < 0 ? 1 : -1), 0));
        }
        else
        {
            player.RB.AddForce(new Vector2(playerData.movementVelocity * xInput * (((player.CurrentVelocity.x < 0 && xInput > 0) || (player.CurrentVelocity.x > 0 && xInput < 0)) ? 2 : 1), 0));
        }
        player.RB.velocity = new Vector2(Mathf.Clamp(player.CurrentVelocity.x, -playerData.movementVelocity, playerData.movementVelocity), 0);

    }
    
}
