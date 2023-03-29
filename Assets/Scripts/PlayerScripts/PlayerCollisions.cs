using System.Net;
using System.Transactions;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCollisions
{
    PlayerController controller;
    public RaycastHit2D hit;
    LayerMask platformMask;

    private Vector2 HitBoxSize
    {
        get
        {
            return new Vector2(controller.BoxCollider.size.x - 0.1f, controller.BoxCollider.size.y - 0.1f);
        }
    }

    public RaycastHit2D BottomHit
    {
        get
        {
            return Physics2D.BoxCast(
            controller.Body.position,
            new Vector2(HitBoxSize.x, 0.1f),
            0.0f,
            Vector2.down,
            HitBoxSize.y * 0.5f,
            platformMask);
        }
    }

    public bool IsBottomColliding
    {
        get
        {
            return BottomHit.collider != null;
        }
    }

    public PlayerCollisions(PlayerController controller)
    {
        this.controller = controller;
        platformMask = LayerMask.GetMask("Platforms");
    }

    public Vector2 UpdateCollisions(Vector2 position)
    {
        if (!IsItColliding(position)) return position;
        position = MovePlayerAwayOnTheXAxis(position);
        return position;
    }

    bool IsItColliding(Vector2 position)
    {
        // Creates a BoxCast in the position the body wants to move.
        hit = Physics2D.BoxCast(
                position,
                HitBoxSize,
                0.0f,
                controller.move.normalized,
                0.1f,
                platformMask);

        // Check if the BoxCast is collided with anything.
        if (hit.collider != null) return true;
        else return false;
    }

    Vector2 MovePlayerAwayOnTheYAxis(Vector2 position)
    {
        float bounds_min_y = position.y - (controller.BoxCollider.size.y / 2);
        float bounds_max_y = position.y + (controller.BoxCollider.size.y / 2);

        if (hit.normal == Vector2.up)
        {
            float difference = bounds_min_y - hit.collider.bounds.max.y;
            difference = Mathf.Abs(difference);
            position += hit.normal * difference;
            controller.move.y = 0f;
        }

        if (hit.normal == Vector2.down)
        {
            float difference = bounds_max_y - hit.collider.bounds.min.y;
            difference = Mathf.Abs(difference);
            position += hit.normal * difference;
            controller.move.y = 0f;
        }

        if (IsItColliding(position)) position = MovePlayerAwayOnTheXAxis(position);
        return position;
    }

    Vector2 MovePlayerAwayOnTheXAxis(Vector2 position)
    {
        float bounds_max_x = position.x + (controller.BoxCollider.size.x / 2);
        float bounds_min_x = position.x - (controller.BoxCollider.size.x / 2);


        if (hit.normal == Vector2.left)
        {
            float difference = bounds_max_x - hit.collider.bounds.min.x;
            difference = Mathf.Abs(difference);
            position += hit.normal * difference;
            controller.move.x = 0f;
        }

        if (hit.normal == Vector2.right)
        {
            float difference = bounds_min_x - hit.collider.bounds.max.x;
            difference = Mathf.Abs(difference);
            position += hit.normal * difference;
            controller.move.x = 0f;
        }

        if (IsItColliding(position)) position = MovePlayerAwayOnTheYAxis(position);
        return position;
    }
}