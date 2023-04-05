using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

namespace marianateixeira.playercontroller
{
    public class PlayerCollisions
    {
        PlayerController controller;
        RaycastHit2D[] hits;
        LayerMask platformMask;

        private Vector2 HitboxSize
        {
            get
            {
                return new Vector2(controller.BoxCollider.size.x - 0.1f, controller.BoxCollider.size.y - 0.1f);
            }
        }

        public bool IsBottomColliding
        {
            get
            {
                RaycastHit2D hit = Physics2D.BoxCast(
                controller.Body.position,
                new Vector2(HitboxSize.x, 0.1f),
                0.0f,
                Vector2.down,
                HitboxSize.y * 0.5f,
                platformMask);

                return hit.collider != null;
            }
        }

        public PlayerCollisions(PlayerController controller)
        {
            this.controller = controller;
            platformMask = LayerMask.GetMask("Platforms");
        }

        public void UpdateCollisions(ref Vector2 position)
        {
            if (!IsItColliding(position)) return;

            foreach(var hit in hits)
            {
                if (hit.normal == Vector2.up || hit.normal == Vector2.down)
                    position = CompensateVerticalAxis(hit, position);
                else if (hit.normal == Vector2.left || hit.normal == Vector2.right)
                    position = CompensateHorizontalAxis(hit, position);
            }

        }

        bool IsItColliding(Vector2 position)
        {
            hits = Physics2D.BoxCastAll(
                    position,
                    HitboxSize,
                    0.0f,
                    controller.Move.normalized,
                    0.1f,
                    platformMask);

            if (hits.Length > 0) return true;
            else return false;
        }

        Vector2 CompensateVerticalAxis(RaycastHit2D hit, Vector2 position)
        {
            float bounds_min_y = position.y - (controller.BoxCollider.size.y / 2);
            float bounds_max_y = position.y + (controller.BoxCollider.size.y / 2);
            float difference = 0.0f;

            if (hit.normal == Vector2.up)
                difference = bounds_min_y - hit.collider.bounds.max.y;
            else if (hit.normal == Vector2.down)
                difference = bounds_max_y - hit.collider.bounds.min.y;

            difference = Mathf.Abs(difference);
            position += hit.normal * difference;
            controller.Move = new Vector2(controller.Move.x, 0f);

            return position;
        }

        Vector2 CompensateHorizontalAxis(RaycastHit2D hit, Vector2 position)
        {
            float bounds_max_x = position.x + (controller.BoxCollider.size.x / 2);
            float bounds_min_x = position.x - (controller.BoxCollider.size.x / 2);
            float difference = 0.0f;

            if (hit.normal == Vector2.left)
                difference = bounds_max_x - hit.collider.bounds.min.x;
            else if (hit.normal == Vector2.right)
                difference = bounds_min_x - hit.collider.bounds.max.x;

            difference = Mathf.Abs(difference);
            position += hit.normal * difference;
            controller.Move = new Vector2(0f, controller.Move.y);

            return position;
        }
    }
}