using System;
using UnityEngine;

namespace marianateixeira.PlayerController
{
    [Serializable]
    public struct PlayerData
    {
        [SerializeField]
        public float maxSpeed;
        [SerializeField]
        public float accelerationTime;
        [SerializeField]
        public float decelerationTime;

        [SerializeField]
        float jumpHeight;
        [SerializeField]
        float jumpTime;
        [SerializeField]
        float fallTime;

        #region Getters for Inspector Variables
        public float MaxSpeed { get { return maxSpeed; } }
        public float AccelerationTime { get { return accelerationTime; } }
        public float DecelerationTime { get { return decelerationTime; } }
        #endregion

        public float JumpGravity { get; private set; }
        public float FallGravity { get; private set; }
        public float InitialJumpVelocity { get; private set; }
        public bool ReadyToJump { get; set; }

        public float RunSpeed { get; set; }
        public float Acceleration { get; private set; }
        public float Deceleration { get; private set; }
        public float AccelerationDirection { get; set; }

        public void Initialize()
        {
            ReadyToJump = true;

            JumpGravity = (-2 * jumpHeight) / (jumpTime * jumpTime);
            FallGravity = (-2 * jumpHeight) / (fallTime * fallTime);

            InitialJumpVelocity = -JumpGravity * jumpTime;

            Acceleration = MaxSpeed / AccelerationTime;
            Deceleration = MaxSpeed / DecelerationTime;
        }
    }

}