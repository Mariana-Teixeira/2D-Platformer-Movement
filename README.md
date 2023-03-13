# 2D Platformer

This Unity Project is a customizable avatar movement for a 2D Platformer, using Unity's version 2021.3.16f1.
It was built to study how 2D Platformer movement might be implemented in a pseudo-professional testing environment, where a designer would want to easily manipulate the "feel" of a character's movement.

The project uses two Finite State Machines to manage the player movement:
- states of the player movement ("IDLE", "RUN", "JUMP", "FALL"),
- states of the player acceleration ("STOP", "ACCELERATE", "MAXSPEED", "DECELETARE").

The project includes a "PlayerController" controller that allows customization through the Unity Inspector of the jump height, jump duration, speed, acceleration, jump buffer and coyote jump of the player GameObject.

"PlayerCollision" manages collisions between the player and a "Platform" layer by initializing four colliders around the given player GameObject and accessing its "PlayerController". When a collision is detected, it checks the player's current direction and applies the opposite force to the player's move vector.
