using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : EntityComponent
{
    #region Check Transforms

    //public Transform GroundCheck { get => groundCheck; private set => groundCheck = value; }

    [SerializeField] private Transform groundCheck;
    //[SerializeField]private Transform wallCheck;
    //[SerializeField]private Transform ledgeCheck;

    #endregion

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;

    [SerializeField] private LayerMask whatIsGround;

    #region Check Functions

    public bool Ground
    {
        get => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    //public bool TouchingWall
    //{
    //    get => Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    //}

    //public bool TouchingLedge
    //{
    //    get => Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    //}

    //public bool TouchingBackWall
    //{
    //    get => Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    //}

    //public Vector2 DetermineCornerPos()
    //{
    //    RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsGround);
    //    float xDistance = xHit.distance;
    //    workspace.Set(xDistance * FacingDirection, 0);
    //    RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)(workspace),Vector2.down,ledgeCheck.position.y - wallCheck.position.y,playerData.whatIsGround);
    //    float yDistance = yHit.distance;

    //    workspace.Set(wallCheck.position.x + (xDistance * FacingDirection), ledgeCheck.position.y - yDistance);
    //    return workspace;
    //}

    #endregion
}
