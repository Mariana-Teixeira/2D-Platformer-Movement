using UnityEngine;

namespace marianateixeira.PlayerController
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        public PlayerData Data;
        public Vector2 Move;

        PlayerMovementStates movementStateMachine;
        PlayerAccelerationStates accelerationStateMachine;
        
        public PlayerCollisions Collisions { get; private set; }

        public Rigidbody2D Body { get; private set; }
        public BoxCollider2D BoxCollider { get; private set; }

        PlayerControls playerInputMap;
        public float RunInput { get; private set; }
        public bool JumpInput { get; private set; }

        void Start()
        {
            GetComponents();
            InititializeComponents();
            EnableInputs();
        }

        void InititializeComponents()
        {
            Data.Initialize();
            movementStateMachine = new PlayerMovementStates(this);
            accelerationStateMachine = new PlayerAccelerationStates(this);
            Collisions = new PlayerCollisions(this);
        }

        void GetComponents()
        {
            Body = GetComponent<Rigidbody2D>();
            BoxCollider = GetComponent<BoxCollider2D>();
        }

        void EnableInputs()
        {
            playerInputMap = new PlayerControls();
            playerInputMap.Enable();
            
            playerInputMap.Player.Jumping.started += context => JumpInput = true;
            playerInputMap.Player.Jumping.canceled += context => JumpInput = false;

            playerInputMap.Player.Running.performed += context => RunInput = context.ReadValue<float>();
            playerInputMap.Player.Running.canceled += context => RunInput = 0;
        }

        private void FixedUpdate()
        {
            accelerationStateMachine.UpdateMachine();
            movementStateMachine.UpdateMachine();

            Vector2 position = Body.position + Move * Time.fixedDeltaTime;
            Collisions.UpdateCollisions(ref position);

            MovePlayer(position);
        }

        void MovePlayer(Vector2 position)
        {
            Body.position = position;
        }
    }
}