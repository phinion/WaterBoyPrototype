using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum State
    {
        idle, running, jumping, falling
    }
    private State state = State.idle;

    private Rigidbody2D rb;
    private Collider2D collider;
    private Animator anim;

    public LayerMask groundLayers;

    public float xInput;
    public float yInput;
    public float turnTimer;
    public float turnTimerSet = 0.1f;

    public float facingDirection = 1;
    public float lastWallJumpDirection;
    public float moveSpeed = 3f;
    public float movementForceInAir = 1f;
    public float airDragMultiplier = 0.95f;
    public float wallSlideSpeed = 3f;

    public float jumpForce = 3f;
    public float variableJumpHeightMultiplier = 0.5f;
    public int amountOfJumps = 1;
    public int amountofJumpsLeft;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimer;
    public float jumpTimerSet = 0.5f;
    public float wallJumpTimer;
    public float wallJumpTimerSet = 0.5f;
    public float wallCheckDistance = 0.4f;

    public float ledgeClimbTimer;
    public float ledgeClimbTimerSet = 0.5f;

    public float ledgeClimbXOffset1 = 0f;
    public float ledgeClimbYOffset1 = 0f;
    public float ledgeClimbXOffset2 = 0f;
    public float ledgeClimbYOffset2 = 0f;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    private Vector2 ledgePosBot;
    private Vector2 ledgePos1;
    private Vector2 ledgePos2;

    public bool canMove;
    public bool canFlip;
    public bool checkJumpMultiplier;
    public bool canNormalJump = false;
    public bool canWallJump = false;
    public bool canClimbLedge = false;
    public bool ledgeDetected;
    public bool isAttemptingToJump = false;
    public bool isFacingRight = true;
    public bool isGrounded = false;
    public bool isTouchingWall = false;
    public bool isTouchingLedge = false;
    public bool isWallSliding = false;
    public bool hasWallJumped;
    //[SerializeField] private bool isCrouched = false;

    private void OnDrawGizmos()
    {
        //Ground check circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.1f);

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + 0.4f, transform.position.y, transform.position.z));

        Vector3 _temp = new Vector3(transform.position.x, transform.position.y + 0.4f,transform.position.z);
        Gizmos.DrawLine(_temp, new Vector3(_temp.x + wallCheckDistance, _temp.y, _temp.z));
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        groundLayers = LayerMask.GetMask("Ground");

        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
    }

    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        AnimationState();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckLedgeClimb();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckSurroundings();
        AppleMovement();
    }

    public void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.1f, groundLayers);

        isTouchingWall = Physics2D.Raycast(transform.position, transform.right, wallCheckDistance, groundLayers);

        isTouchingLedge = !Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.4f), transform.right, wallCheckDistance, groundLayers);

        if(isTouchingWall && isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = transform.position;
        }

    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            amountofJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall)
        {
            canWallJump = true;
        }

        if (amountofJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }

    }

    private void CheckLedgeClimb()
    {
        if(ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;

            if (isFacingRight)
            {
                ledgePos1 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) - ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Floor(ledgePosBot.x + wallCheckDistance) + ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) + ledgeClimbXOffset1, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2(Mathf.Ceil(ledgePosBot.x - wallCheckDistance) - ledgeClimbXOffset2, Mathf.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }

            canMove = false;
            canFlip = false;

            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;

            //anim.SetBool("canClimbLedge", canClimbLedge);
            transform.position = ledgePos1;
            ledgeClimbTimer = ledgeClimbTimerSet;
        }

        //if (canClimbLedge)
        //{
        //    transform.position = ledgePos1;
        //}

    }
    public void FinishLedgeClimb()
    {
        rb.gravityScale = 3;
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        anim.SetBool("canClimbLedge", canClimbLedge);
    }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && xInput == facingDirection && rb.velocity.y < 0 && !canClimbLedge)
        {

            isWallSliding = true;

        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            //if(isGrounded || amountofJumpsLeft > 0 && isTouchingWall)
            //{
            //    NormalJump();
            //}
            //else
            //{
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            //}
        }

        if(Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if(!isGrounded && xInput != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if (turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if(turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        //variable jump height
        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && rb.velocity.x < -0.1f)
        {
            Flip();
        }
        else if (!isFacingRight && rb.velocity.x > 0.1f)
        {
            Flip();
        }
    }

    private void Flip()
    {
        if (!isWallSliding && canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }



    private void AppleMovement()
    {
        //mid air movement
        if (!isGrounded && !isWallSliding && xInput == 0)
        {
            Debug.Log("air drag applying");
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        //ledge climb
        else if (canClimbLedge)
        {

            //climb ledge
            if(xInput == facingDirection)
            {
                ledgeClimbTimer -= Time.deltaTime;

                if (ledgeClimbTimer < 0)
                {
                    anim.SetBool("canClimbLedge", canClimbLedge);
                }
            }
            //fall down
            else
            {
                ledgeClimbTimer = ledgeClimbTimerSet;

                rb.gravityScale = 3;
                canClimbLedge = false;
                canMove = true;
                canFlip = true;
                ledgeDetected = false;
            }
        }
        else if (canMove && isGrounded && state != State.jumping)
        {
            Debug.Log("normal mvoement");
            rb.velocity = new Vector2(moveSpeed * xInput, rb.velocity.y);

        }
        else if (!isGrounded && !isWallSliding && xInput != 0)
        {

            Debug.Log("movement " + movementForceInAir * xInput);

            Vector2 forceToAdd = new Vector2(movementForceInAir * xInput, 0);
            rb.AddForce(forceToAdd);

            Debug.Log("adding force......................... " + forceToAdd);

            if (Mathf.Abs(rb.velocity.x) > moveSpeed * 2f)
            {
                rb.velocity = new Vector2(moveSpeed * 2f * xInput, rb.velocity.y);
            }
        }

        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }

        //Debug.Log("velocity: " + rb.velocity);
    }

    private void CheckJump()
    {

        if(jumpTimer > 0)
        {
            //walljump
            if(!isGrounded && isTouchingWall && Mathf.Abs(xInput) >= 0.1 && xInput != facingDirection)
            {
                WallJump();
            } 
            else if (isGrounded)
            {
                NormalJump();
            }
        }

        if(isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }

        if(wallJumpTimer > 0)
        {
            //breaks jump force if you try to turn the other way
            if(hasWallJumped && xInput == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                hasWallJumped = false;

            } else if(wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }

    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            Vector2 yForce = new Vector2(0, jumpForce);
            rb.AddForce(yForce, ForceMode2D.Impulse);

            //jump
            if (xInput != 0)
            {
                //Debug.Log("water boy is leaping");
                rb.velocity = new Vector2(moveSpeed * xInput * 2f, rb.velocity.y);
                Debug.Log("water boy is leaping w velocity x at " + rb.velocity.x);
            }

            //Debug.Log("Jump");

            //leap
            state = State.jumping;
            amountofJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        //else if(isWallSliding && xInput == 0 && canJump)
        //{
        //    isWallSliding = false;
        //    amountofJumpsLeft--;
        //    Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection,wallHopForce* wallHopDirection.y);
        //    rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        //}

        if (canWallJump)
        {
            Debug.Log("wall Jump");
            rb.velocity = new Vector2(rb.velocity.x, 0);

            isWallSliding = false;
            amountofJumpsLeft = amountOfJumps;
            amountofJumpsLeft--;

            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * xInput, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);

            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
        }
    }

    public void AnimationState()
    {
        if (rb.velocity.y < 0 && !isGrounded)
        {
            state = State.falling;
        }
        else if (state == State.jumping)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {

            if (isGrounded)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }

        anim.SetInteger("state", (int)state);
    }



}
