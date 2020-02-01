using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacter : MonoBehaviour
{
    // Horizontal movement properties
    public float walkSpeed = 10.0f;

    // Vertical movement properties
    public float gravityAcceleration = 0.5f;
    public float terminalVelocity = 9.0f;
    public float jumpVelocity = 4.0f;

    // Physics properties
    public float minimumSeparation = 0.5f;
    public bool doPositionSmoothing = true;

    // Components
    private BoxCollider2D characterCollider;

    // Private physics
    private Vector2 velocity = new Vector2(0.0f, 0.0f);
    private bool isGrounded = false;

    // Interpolation
    private Vector2 previousPosition;
    private Vector2 currentPosition;
    private float previousTime;

    void Start()
    {
        characterCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        InterpolatePosition();
    }

    void FixedUpdate()
    {
        // Set last fixed update
        previousPosition = currentPosition;
        previousTime = Time.fixedTime;

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
            RaycastHit2D hitResult = Physics2D.BoxCast(currentPosition, characterCollider.size, 0.0f, new Vector2(horizontalDirection, 0.0f), Mathf.Abs(horizontalWalk), LayerMask.GetMask("World"));

            // If we hit something, we can only move the distance of the raycast, minus the minimum separation (to prevent getting stuck in walls)
            if (hitResult)
            {
                horizontalWalk = (hitResult.distance - minimumSeparation) * horizontalDirection;
            }

            // Apply movement
            currentPosition.x += horizontalWalk;
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
        RaycastHit2D hitResult = Physics2D.BoxCast(currentPosition, characterCollider.size, 0.0f, new Vector2(0.0f, verticalDirection), Mathf.Abs(verticalMove), LayerMask.GetMask("World"));

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
        currentPosition.y += verticalMove;
    }

    private void InterpolatePosition()
    {
        if (doPositionSmoothing)
        {
            // Figure out how much of a step has elapsed since position update
            float timeSinceUpdate = Time.time - previousTime;
            float alpha = timeSinceUpdate / Time.fixedDeltaTime;

            // Interpolate position
            Vector2 interpolatedPosition = Vector2.Lerp(previousPosition, currentPosition, alpha);
            transform.position = new Vector3(interpolatedPosition.x, interpolatedPosition.y, 0.0f);
        }
        else
        {
            // Just set most recent position
            transform.position = new Vector3(currentPosition.x, currentPosition.y);
        }
    }
}
