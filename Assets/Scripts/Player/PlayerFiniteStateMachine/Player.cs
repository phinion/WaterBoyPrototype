using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    //public PlayerWallSlideState WallSlideState { get; private set; }
    //public PlayerWallGrabState WallGrabState { get; private set; }
    //public PlayerWallClimbState WallClimbState { get; private set; }
    //public PlayerWallJumpState WallJumpState { get; private set; }
    //public PlayerLedgeClimbState LedgeClimbState { get; private set; }

    [SerializeField]
    private PlayerData playerData;

    #endregion

    #region Components

    public Entity Entity { get; private set; }
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }
    #endregion

    #region UnityCallbackFunctions

    private void Awake()
    {
        Entity = GetComponentInChildren<Entity>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        //WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        //WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
        //WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
        //WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "wallJump");
        //LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimb");


    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Entity.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion


    #region Other Functions

   

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    //private void Flip()
    //{
    //    FacingDirection *= -1;
    //    transform.Rotate(0.0f, 180.0f, 0.0f);
    //}

    #endregion
}
