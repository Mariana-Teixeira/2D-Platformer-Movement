using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

// TODO: Improve documentation of code.
// TODO: Allow CoyoteTime/JumpBuffer to use ElapsedTime.
// TODO: Use ElapsedTime instead of FixedFramerate.

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    public bool attemptingToJump;

    PlayerCollisions playerCollisions;
    PlayerMovementStates playerMovement;
    PlayerAccelerationStates playerAcceleration;

    Rigidbody2D body;
    BoxCollider2D boxCollider;
    [SerializeField] Camera playerCamera;

    PlayerControls playerInputMap;
    float runInput;
    bool jumpInput;

    public Vector2 move;

    [SerializeField] float timeToJumpApex;
    [SerializeField] float timeToGround;
    [SerializeField] float jumpHeight;
    float jumpGravity;
    float fallGravity;
    float initialJumpVelocity;

    [SerializeField] float maxSpeed;
    [SerializeField] float timeToMaxSpeed;
    [SerializeField] float timeToStop;
    float runSpeed;
    float acceleration;
    float deceleration;
    float accelerationDirection;


    #region Properties
    public Rigidbody2D Body
    {
        get
        {
            return body;
        }
    }
    public BoxCollider2D BoxCollider
    {
        get
        {
            return boxCollider;
        }
    }
    public PlayerCollisions Collisions
    {
        get
        {
            return playerCollisions;
        }
    }
    public float RunInput
    {
        get
        {
            return runInput;
        }
    }
    public bool JumpInput
    {
        get
        {
            return jumpInput;
        }
    }
    public float JumpGravity
    {
        get
        {
            return jumpGravity;
        }
    }
    public float FallGravity
    {
        get
        {
            return fallGravity;
        }
    }
    public float InitialJumpVelocity
    {
        get
        {
            return initialJumpVelocity;
        }
    }
    public float RunSpeed
    {
        get
        {
            return runSpeed;
        }
        set
        {
            runSpeed = value;
        }
    }
    public float Acceleration
    {
        get
        {
            return acceleration;
        }
    }
    public float Deceleration
    {
        get
        {
            return deceleration;
        }
    }
    public float AccelerationDirection
    {
        get
        {
            return accelerationDirection;
        }
        set
        {
            accelerationDirection = value;
        }
    }
    public float MaxSpeed
    {
        get
        {
            return maxSpeed;
        }
    }

    public PlayerControls _PlayerControls
    {
        get
        {
            return playerInputMap;
        }
    }
    #endregion

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        playerInputMap = new PlayerControls();
        playerInputMap.Enable();

        float jumpApex = timeToJumpApex / 2;
        float groundApex = timeToGround / 2;
        jumpGravity  = (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex);
        fallGravity  = (-2 * jumpHeight) / (timeToGround * timeToGround);
        initialJumpVelocity = -jumpGravity * timeToJumpApex;
        acceleration = maxSpeed / timeToMaxSpeed;
        deceleration = maxSpeed / timeToStop;

        playerMovement = new PlayerMovementStates(this);
        playerCollisions = new PlayerCollisions(this);
        playerAcceleration = new PlayerAccelerationStates(this);

        playerInputMap.Player.Jumping.started += context => jumpInput = true;
        playerInputMap.Player.Jumping.canceled += context => jumpInput = false;

        playerInputMap.Player.Running.performed += context => runInput = context.ReadValue<float>();
        playerInputMap.Player.Running.canceled += context => runInput = 0;
    }

    private void FixedUpdate()
    {
        playerAcceleration.UpdateMachine();
        playerMovement.UpdateMachine();

        Vector2 position = body.position + move * Time.fixedDeltaTime;
        position = playerCollisions.UpdateCollisions(position);

        MovePlayer(position);
    }

    void MovePlayer(Vector2 position)
    {
        body.position = position;
    }

    void OnDrawGizmos()
    {
        if (playerCollisions == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(playerCollisions.hit.point, 0.1f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(playerCollisions.hit.point, playerCollisions.hit.normal);
    }
}