using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

// TODO: Comment on more functions so the code become more clear to read.
// TODO: Add a requirement for attaching this script to an object (requires Rigidbody2D, BoxCollider, etc...)
// TODO: I want CoyoteTime and JumpBuffer to be counted in frames independent of framerate.

public class PlayerController : MonoBehaviour
{
    PlayerCollisions playerCollisions;
    PlayerMovementStates playerMovement;
    PlayerAccelerationStates playerAcceleration;

    Rigidbody2D body;
    BoxCollider2D boxCollider;
    [SerializeField] Camera playerCamera;

    PlayerControls playerControls;
    float runInput;
    bool jumpInput;

    public Vector2 move; // I can't figure out how to make it a private variable.

    [SerializeField] float jumpDuration;
    [SerializeField] float fallDuration;
    [SerializeField] float jumpHeight;
    float jumpGravity;
    float fallGravity;
    float initialJumpVelocity;

    [SerializeField] float maxSpeed;
    [SerializeField] float timeToMaxSpeed;
    float runSpeed;
    float acceleration;
    float accelerationDirection;

    int coyoteFrame;
    [SerializeField] int coyoteCounter;
    int jumpBufferFrame;
    [SerializeField] int jumpBufferCounter;


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
        set
        {
            jumpInput = value;
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
    public bool IsCoyoteTimeInEffect
    {
        get
        {
            return coyoteFrame <= 0 ? false : true;
        }
    }
    public bool IsJumpBufferInEffect
    {
        get
        {
            return jumpBufferFrame <= 0 ? false : true;
        }
    }
    #endregion

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        playerControls = new PlayerControls();
        playerControls.Enable();

        float jumpApex = jumpDuration / 2;
        float fallApex = fallDuration / 2;
        jumpGravity = (-2 * jumpHeight) / Mathf.Pow(jumpApex, 2);
        fallGravity = (-2 * jumpHeight) / Mathf.Pow(fallApex, 2);
        initialJumpVelocity = -jumpGravity * jumpApex;

        acceleration = maxSpeed / timeToMaxSpeed;

        playerMovement = new PlayerMovementStates(this);
        playerCollisions = new PlayerCollisions(this);
        playerAcceleration = new PlayerAccelerationStates(this);

        // What a mess...
        playerControls.Player.Jumping.started += context => JumpInput = true;
        playerControls.Player.Jumping.canceled += context => jumpInput = false;

        playerControls.Player.Running.performed += context => runInput = context.ReadValue<float>();
        playerControls.Player.Running.canceled += context => runInput = 0;
    }

    void Update()
    {
        TickTimers();
    }

    void TickTimers()
    {
        if (!playerCollisions.IsBottomColliding)
        {
            TickJumpBuffer();
            TickCoyoteTimer();
        }
    }

    private void FixedUpdate() // Why could ellapsed time be important here? (Research more.)
    {
        playerAcceleration.UpdateMachine(); // Calculates the horizontal acceleration of the player for the 'move' vector.
        playerMovement.UpdateMachine(); // Defines the State the player is in, which influences the 'move' vector.
        playerCollisions.UpdateCollisions(); // Collision Calculation for the 'move' vector
        MovePlayer();
    }

    void MovePlayer()
    {
        body.position = body.position + (move * Time.fixedDeltaTime);
    }

    public void TickCoyoteTimer()
    {
        coyoteFrame -= 1;
    }

    public void ResetCoyoteTimer()
    {
        coyoteFrame = coyoteCounter;
    }

    public void TickJumpBuffer()
    {
        jumpBufferFrame -= 1;
    }

    public void ResetJumpBufferTimer()
    {
        jumpBufferFrame = jumpBufferCounter;
    }
}