using UnityEngine;

// TODO: Comment on more functions so the code become more clear to read.
// TODO: Add a requirement for attaching this script to an object (requires Rigidbody2D, BoxCollider, etc...)
// TODO: I want CoyoteTime and JumpBuffer to be counted in frames independent of framerate.
// TODO: Avoid using FixedFramerate, used ElapsedTime instead.

public class PlayerController : MonoBehaviour
{
    PlayerCollisions playerCollisions;
    PlayerMovementStates playerMovement;
    PlayerAccelerationStates playerAcceleration;

    Rigidbody2D body;
    BoxCollider2D boxCollider;
    [SerializeField] Camera playerCamera;

    PlayerControls playerInputMap;
    float runInput;
    bool jumpInput;

    public Vector2 move; // I can't figure out how to make it a private variable.

    [SerializeField] float jumpDuration;
    [SerializeField] float jumpHeight;
    float gravity;
    [SerializeField] float gravityMultiplier;
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
    public float Gravity
    {
        get
        {
            return gravity;
        }
    }
    public float gravityModifier
    {
        get
        {
            return gravityModifier;
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

        float jumpApex = jumpDuration / 2;
        gravity  = (-2 * jumpHeight) / Mathf.Pow(jumpApex, 2);
        initialJumpVelocity = -gravity * jumpApex;
        acceleration = maxSpeed / timeToMaxSpeed;

        playerMovement = new PlayerMovementStates(this);
        playerCollisions = new PlayerCollisions(this);
        playerAcceleration = new PlayerAccelerationStates(this);

        playerInputMap.Player.Jumping.started += context => jumpInput = true;
        playerInputMap.Player.Jumping.canceled += context => jumpInput = false;

        playerInputMap.Player.Running.performed += context => runInput = context.ReadValue<float>();
        playerInputMap.Player.Running.canceled += context => runInput = 0;
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

    private void FixedUpdate()
    {
        playerAcceleration.UpdateMachine();
        playerMovement.UpdateMachine();
        playerCollisions.UpdateCollisions();
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