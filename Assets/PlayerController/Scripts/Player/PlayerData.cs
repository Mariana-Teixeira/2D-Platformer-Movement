using System;
using System.IO;
using UnityEngine;

namespace marianateixeira.PlayerController
{
    [Serializable]
    public struct PlayerData
    {
        [SerializeField]
        public bool variableJump;

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

        public void SaveDataToFile()
        {
            SaveData saveData = new SaveData()
            {
                VariableJump = variableJump,
                MaxSpeed = maxSpeed,
                AccelerationTime = accelerationTime,
                DecelerationTime = decelerationTime,
                JumpHeight = jumpHeight,
                JumpTime = jumpTime,
                FallTime = fallTime,
            };

            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(Application.dataPath + "/playerConfig.json", json);
        }

        public void LoadDataFromFile()
        {
            string json = File.ReadAllText(Application.dataPath + "/playerConfig.json");
            SaveData jsonData = JsonUtility.FromJson<SaveData>(json);

            maxSpeed = jsonData.MaxSpeed;
            accelerationTime = jsonData.AccelerationTime;
            decelerationTime = jsonData.DecelerationTime;
            variableJump = jsonData.VariableJump;
            jumpHeight = jsonData.JumpHeight;
            jumpTime = jsonData.JumpTime;
            fallTime = jsonData.FallTime;
        }

        private class SaveData
        {
            public bool VariableJump;
            public float MaxSpeed;
            public float AccelerationTime;
            public float DecelerationTime;
            public float JumpHeight;
            public float JumpTime;
            public float FallTime;
        }
    }
}