using UnityEngine;

namespace marianateixeira.playercontroller
{
    public class PlayerAccelerationStates
    {
        PlayerController controller;

        public enum State { STOP, ACCELERATE, MAXSPEED, DECELETARE, NULL };
        State currentState;

        public PlayerAccelerationStates(PlayerController controller)
        {
            this.controller = controller;

            currentState = State.STOP;
        }

        public void UpdateMachine()
        {
            TickState();
            if (CheckingStateConditions() != State.NULL) ChangeState(CheckingStateConditions());
        }

        public State CheckingStateConditions()
        {
            switch (currentState)
            {
                case State.STOP:
                    if (controller.RunInput != 0) return State.ACCELERATE;
                    break;

                case State.ACCELERATE:
                    if (controller.RunInput != controller.Data.AccelerationDirection) return State.DECELETARE;
                    if (controller.Data.RunSpeed >= controller.Data.MaxSpeed) return State.MAXSPEED;
                    break;

                case State.MAXSPEED:
                    if (controller.RunInput != controller.Data.AccelerationDirection) return State.DECELETARE;
                    break;

                case State.DECELETARE:
                    if (controller.RunInput == controller.Data.AccelerationDirection) return State.ACCELERATE;
                    if (controller.Data.RunSpeed <= 0.05f) return State.STOP;
                    break;
            }

            return State.NULL;
        }

        public void ChangeState(State state)
        {
            currentState = state;
            EnterState();
        }

        public void EnterState()
        {
            switch (currentState)
            {
                case State.ACCELERATE:
                    if (controller.RunInput != 0) controller.Data.AccelerationDirection = controller.RunInput;
                    break;
            }
        }

        public void TickState()
        {
            switch (currentState)
            {
                case State.STOP:
                    controller.Data.RunSpeed = 0f;
                    break;

                case State.ACCELERATE:
                    controller.Data.RunSpeed += controller.Data.Acceleration * Time.fixedDeltaTime;
                    break;

                case State.MAXSPEED:
                    controller.Data.RunSpeed = controller.Data.MaxSpeed;
                    break;

                case State.DECELETARE:
                    controller.Data.RunSpeed -= controller.Data.Deceleration * Time.fixedDeltaTime;
                    break;
            }


        }
    }

}
