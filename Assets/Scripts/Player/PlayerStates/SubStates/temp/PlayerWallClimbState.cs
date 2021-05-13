//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerWallClimbState : PlayerTouchingWallState
//{
//    public PlayerWallClimbState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
//    {
//    }

//    public override void LogicUpdate()
//    {
//        base.LogicUpdate();

//        if (!isExitingState)
//        {
//            entity.Movement.SetVelocityY(playerData.wallClimbVelocity);

//            if (yInput != 1)
//            {
//                stateMachine.ChangeState(player.WallGrabState);
//            }
//        }


//    }
//}
