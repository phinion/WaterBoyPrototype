//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerWallJumpState : PlayerAbilityState
//{
//    private int wallJumpDirection;

//    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
//    {
//    }

//    public override void Enter()
//    {
//        base.Enter();

//        player.InputHandler.UseJumpInput();
//        player.JumpState.ResetAmountOfJumpsLeft();
//        entity.Movement.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
//        entity.Movement.CheckIfShouldFlip(wallJumpDirection);
//        player.JumpState.DecreaseAmountOfJumpsLeft();
//    }

//    public override void LogicUpdate()
//    {
//        base.LogicUpdate();

//        //player.Anim.SetFloat("yVelocity", entity.Movement.CurrentVelocity.y);
//        //player.Anim.SetFloat("xVelocity", entity.Movement.CurrentVelocity.x);

//        if(Time.time >= startTime + playerData.wallJumpTime)
//        {
//            isAbilityDone = true;
//        }
//    }

//    public void DetermineWallJumpDirection(bool isTouchingWall)
//    {
//        if (isTouchingWall)
//        {
//            wallJumpDirection = -entity.Movement.FacingDirection;
//        }
//        else
//        {
//            wallJumpDirection = entity.Movement.FacingDirection;
//        }
//    }
//}
