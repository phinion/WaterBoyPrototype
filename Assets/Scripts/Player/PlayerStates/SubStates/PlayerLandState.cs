using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            if (xInput != 0)
            {
                stateMachine.ChangeState(player.MoveState);
            } 
            //else if(yInput <0)
            //{
            //    stateMachine.ChangeState(player.SlideState);
            //}
            else if (isAnimationFinished)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(Mathf.Abs(player.CurrentVelocity.x) > 0.01f)
        {
            player.RB.AddForce(new Vector2(playerData.movementVelocity * 2 * (player.CurrentVelocity.x < 0 ? 1 : -1), 0));
        }
    }
}
