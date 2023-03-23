using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementStates
{
    PlayerController controller;

    public enum State { IDLE, RUN, JUMP, FALL, NULL };
    State currentState;

    public PlayerMovementStates(PlayerController controller)
    {
        this.controller = controller;
        currentState = State.IDLE;

        /*controller._PlayerControls.Player.Jumping.started += context =>
         * if (currentState == State.IDLE || currentState == State.RUN) return State.Jump;*/
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
                else if (controller.JumpInput) return State.JUMP; // You would check JustJumped
                else if (controller.RunSpeed != 0) return State.RUN;
                break;

            case State.RUN:
                if (!controller.IsCoyoteTimeInEffect) return State.FALL;
                else if (controller.JumpInput) return State.JUMP;
                else if (controller.RunSpeed == 0 && controller.RunInput == 0) return State.IDLE;
                break;

            case State.JUMP:
                if (controller.move.y < 0 || !controller.JumpInput) return State.FALL;
                else if (controller.Collisions.IsBottomColliding) return State.IDLE;
                break;

            case State.FALL:
                if (controller.IsJumpBufferInEffect && controller.Collisions.IsBottomColliding) return State.JUMP;
                else if (controller.Collisions.IsBottomColliding && controller.move.x != 0) return State.RUN;
                else if (controller.Collisions.IsBottomColliding) return State.IDLE;
                break;
        }

        return State.NULL;
    }

    public void ChangeState(State state)
    {
        ExitState();
        currentState = state;
        EnterState();
    }

    public void EnterState()
    {
        switch (currentState)
        {
            case State.JUMP:
                controller.move.y = controller.InitialJumpVelocity;
                break;
        }
    }

    public void TickState()
    {
        switch (currentState)
        {
            case State.RUN:
                controller.move.x = controller.AccelerationDirection * controller.RunSpeed;
                break;

            case State.JUMP:
                controller.move.x = controller.AccelerationDirection * controller.RunSpeed;
                controller.move.y += controller.Gravity * Time.fixedDeltaTime;
                break;

            case State.FALL:
                if (controller.JumpInput) controller.ResetJumpBufferTimer();
                controller.move.x = controller.AccelerationDirection * controller.RunSpeed;
                controller.move.y += controller.Gravity * controller.gravityModifier * Time.fixedDeltaTime;
                break;
        }
    }

    public void ExitState()
    {
        switch(currentState)
        {
            case State.FALL:
                controller.ResetCoyoteTimer();
                controller.ResetJumpBufferTimer();
                break;
        }
    }
}
