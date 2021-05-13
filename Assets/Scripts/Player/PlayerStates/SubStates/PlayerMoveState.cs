using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.CheckIfShouldFlip(xInput);

        //player.SetVelocityX(playerData.movementVelocity * xInput);

        if(Mathf.Abs(player.RB.velocity.x) < 0.01f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (xInput == 0)
        {
            player.RB.AddForce(new Vector2(playerData.movementVelocity * (player.RB.velocity.x < 0 ? 1 : -1), 0));
        }
        else
        {
            player.RB.AddForce(new Vector2(playerData.movementVelocity * xInput, 0));
        }
        player.RB.velocity = new Vector2(Mathf.Clamp(player.RB.velocity.x, -playerData.movementVelocity, playerData.movementVelocity),0);
    }
}
