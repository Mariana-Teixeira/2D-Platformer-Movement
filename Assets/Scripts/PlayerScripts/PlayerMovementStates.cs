using UnityEngine;

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
                else if (controller.JumpInput && !controller.attemptingToJump) return State.JUMP;
                else if (controller.RunSpeed != 0) return State.RUN;
                break;

            case State.RUN:
                if (!controller.Collisions.IsBottomColliding) return State.FALL;
                else if (controller.JumpInput && !controller.attemptingToJump) return State.JUMP;
                else if (controller.RunSpeed == 0 && controller.RunInput == 0) return State.IDLE;
                break;

            case State.JUMP:
                if (controller.move.y < 0 || !controller.JumpInput) return State.FALL;
                else if (controller.Collisions.IsBottomColliding) return State.IDLE;
                break;

            case State.FALL:
                if (controller.Collisions.IsBottomColliding && controller.move.x != 0) return State.RUN;
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
                controller.move.x = 0.0f;
                break;
            case State.JUMP:
                controller.attemptingToJump = true;
                controller.move.y = controller.InitialJumpVelocity;
                break;
        }
    }

    public void TickState()
    {
        switch (currentState)
        {
            case State.IDLE:
                if (!controller.JumpInput) controller.attemptingToJump = false;
                break;
            case State.RUN:
                if (!controller.JumpInput) controller.attemptingToJump = false;
                controller.move.x = controller.AccelerationDirection * controller.RunSpeed;
                break;

            case State.JUMP:
                controller.move.x = controller.AccelerationDirection * controller.RunSpeed;
                controller.move.y += controller.JumpGravity * Time.fixedDeltaTime;
                break;

            case State.FALL:
                controller.move.x = controller.AccelerationDirection * controller.RunSpeed;
                controller.move.y += controller.FallGravity * Time.fixedDeltaTime;
                break;
        }
    }
}
