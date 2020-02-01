using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    public float walkSpeed = 10.0f;

    public float gravityAcceleration = 0.5f;
    public float terminalVelocity = 9.0f;
    public float jumpVelocity = 4.0f;

    public float minimumSeparation = 0.5f;

    private BoxCollider2D characterCollider;

    private Vector2 velocity = new Vector2(0.0f, 0.0f);
    private bool isGrounded = false;

    void Start()
    {
        characterCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        // Split vertical and horizontal moves to avoid catching on the ground
        MoveHorizontal();
        MoveVertical();
    }

    private void MoveHorizontal()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal_1");
        float horizontalWalk = horizontalInput * walkSpeed * Time.fixedDeltaTime;

        // Only attempt move if there is some input
        if (horizontalWalk != 0.0f)
        {
            // Raycast against "World" objects
            float horizontalDirection = horizontalWalk < 0.0f ? -1.0f : 1.0f;
            RaycastHit2D hitResult = Physics2D.BoxCast(transform.position, characterCollider.size, 0.0f, new Vector2(horizontalDirection, 0.0f), Mathf.Abs(horizontalWalk), LayerMask.GetMask("World"));

            // If we hit something, we can only move the distance of the raycast, minus the minimum separation (to prevent getting stuck in walls)
            if (hitResult)
            {
                horizontalWalk = (hitResult.distance - minimumSeparation) * horizontalDirection;
            }

            // Apply movement
            transform.Translate(horizontalWalk, 0.0f, 0.0f);
        }
    }

    private void MoveVertical()
    {
        // Only jump if we're grounded
        if (Input.GetButton("Jump_1") && isGrounded)
        {
            velocity.y = jumpVelocity;
        }

        // Reset grounded state after all checks
        isGrounded = false;

        // Accelerate according to gravity and limit by terminal velocity
        velocity.y -= gravityAcceleration * Time.fixedDeltaTime;
        velocity.y = Mathf.Min(velocity.y, terminalVelocity);

        float verticalMove = velocity.y * Time.fixedDeltaTime;
        float verticalDirection = velocity.y < 0.0f ? -1.0f : 1.0f;

        // Raycast against "World" objects
        RaycastHit2D hitResult = Physics2D.BoxCast(transform.position, characterCollider.size, 0.0f, new Vector2(0.0f, verticalDirection), Mathf.Abs(verticalMove), LayerMask.GetMask("World"));

        // If we hit something, we can only move the distance of the raycast, minus the minimum separation (to prevent getting stuck in walls)
        if (hitResult)
        {
            verticalMove = (hitResult.distance - minimumSeparation) * verticalDirection;

            // Set grounded if we hit something below us
            if (verticalDirection < 0.0f)
            {
                isGrounded = true;

                // Limit velocity on landing
                velocity.y = Mathf.Max(0.0f, velocity.y);
            }
        }

        // Actually apply the movement
        transform.Translate(0.0f, verticalMove, 0.0f);
    }
}
