using UnityEngine;

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
                if (controller.RunInput != controller.AccelerationDirection) return State.DECELETARE;
                if (controller.RunSpeed >= controller.MaxSpeed) return State.MAXSPEED;
                break;

            case State.MAXSPEED:
                if (controller.RunInput != controller.AccelerationDirection) return State.DECELETARE;
                break;

            case State.DECELETARE:
                if (controller.RunInput == controller.AccelerationDirection) return State.ACCELERATE;
                if (controller.RunSpeed <= 0.05f) return State.STOP;
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
                if (controller.RunInput != 0) controller.AccelerationDirection = controller.RunInput;
                break;
        }
    }

    public void TickState()
    {
        switch (currentState)
        {
            case State.STOP:
                controller.RunSpeed = 0;
                break;

            case State.ACCELERATE:
                controller.RunSpeed += controller.Acceleration * Time.fixedDeltaTime;
                break;

            case State.MAXSPEED:
                controller.RunSpeed = controller.MaxSpeed;
                break;

            case State.DECELETARE:
                controller.RunSpeed -= controller.Deceleration * Time.fixedDeltaTime;
                break;
        }


    }
}
