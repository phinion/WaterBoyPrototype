using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int amountsOfJumpsLeft;

    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, PlayerData _playerData, string _animBoolName) : base(_player, _stateMachine, _playerData, _animBoolName)
    {
        amountsOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseJumpInput();
        entity.Movement.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true;
        amountsOfJumpsLeft--;
        player.InAirState.SetIsJumping();
    }

    public bool CanJump()
    {

        if(amountsOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void ResetAmountOfJumpsLeft() =>amountsOfJumpsLeft = playerData.amountOfJumps;


    public void DecreaseAmountOfJumpsLeft() => amountsOfJumpsLeft--;

}
