using UnityEngine;

namespace marianateixeira.playercontroller
{
    public class PlayerMovementStates
    {
        PlayerController controller;

        public enum State { IDLE, RUN, JUMP, FALL, NULL };
        State currentState;

        public PlayerMovementStates(PlayerController controller)
        {
            this.controller = controller;

            currentState = State.IDLE;
        }

        public void UpdateMachine()
        {
            if (CheckingStateConditions() != State.NULL) ChangeState(CheckingStateConditions());
            TickState();
        }

        public State CheckingStateConditions()
        {
            switch (currentState)
            {
                case State.IDLE:
                    if (!controller.Collisions.IsBottomColliding) return State.FALL;
                    else if (controller.JumpInput && controller.Data.ReadyToJump) return State.JUMP;
                    else if (controller.Data.RunSpeed != 0) return State.RUN;
                    break;

                case State.RUN:
                    if (!controller.Collisions.IsBottomColliding) return State.FALL;
                    else if (controller.JumpInput && controller.Data.ReadyToJump) return State.JUMP;
                    else if (controller.Data.RunSpeed == 0 && controller.RunInput == 0) return State.IDLE;
                    break;

                case State.JUMP:
                    if (controller.Move.y < 0 || !controller.JumpInput) return State.FALL;
                    else if (controller.Collisions.IsBottomColliding) return State.IDLE;
                    break;

                case State.FALL:
                    if (controller.Collisions.IsBottomColliding && controller.Move.x != 0) return State.RUN;
                    else if (controller.Collisions.IsBottomColliding) return State.IDLE;
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
                case State.IDLE:
                    controller.Move = new Vector2(0.0f, controller.Move.y);
                    break;
                case State.JUMP:
                    controller.Data.ReadyToJump = false;
                    controller.Move.y = controller.Data.InitialJumpVelocity;
                    break;
            }
        }

        public void TickState()
        {
            switch (currentState)
            {
                case State.IDLE:
                    if (!controller.JumpInput) controller.Data.ReadyToJump = true;
                    break;
                case State.RUN:
                    if (!controller.JumpInput) controller.Data.ReadyToJump = true;
                    controller.Move.x = controller.Data.AccelerationDirection * controller.Data.RunSpeed;
                    break;

                case State.JUMP:
                    controller.Move.x = controller.Data.AccelerationDirection * controller.Data.RunSpeed;
                    controller.Move.y += controller.Data.JumpGravity * Time.fixedDeltaTime;
                    break;

                case State.FALL:
                    controller.Move.x = controller.Data.AccelerationDirection * controller.Data.RunSpeed;
                    controller.Move.y += controller.Data.FallGravity * Time.fixedDeltaTime;
                    break;
            }
        }
    }

}
