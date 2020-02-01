using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Horizontal movement properties
    public float walkSpeed = 10.0f;

    // Vertical movement properties
    public float gravityAcceleration = 0.5f;
    public float terminalVelocity = 9.0f;
    public float jumpVelocity = 4.0f;
    public float groundDistance = 0.02f;

    // Physics properties
    public float minimumSeparation = 0.01f;
    public bool doPositionSmoothing = true;

    // Components
    private BoxCollider2D characterCollider;

    // Private physics
    private Vector2 velocity = new Vector2(0.0f, 0.0f);
    private bool isGrounded = false;

    // Public physics
    public LayerMask m_LayerMask;

    // Interpolation
    private Vector2 previousPosition;
    private Vector2 currentPosition;
    private float previousTime;

    // Input buffer
    private bool wantsToJump = false;

    // Game Logic
    public int points = 0;
    public int location = 0;
    public bool isStunned = false;
    public double lastDamaged = Constants.TIME_STUNNED;

    void Awake()
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

        if (isStunned)
        {
            lastDamaged = lastDamaged - 0.02;
            if (lastDamaged == 0)
            {
                isStunned = false;
                lastDamaged = Constants.TIME_STUNNED;
            }
        } else {
            // Split vertical and horizontal moves to avoid catching on the ground
            MoveHorizontal();
            MoveVertical();
        }

    }

    public void OnHorizontal(InputValue value)
    {
        velocity.x = value.Get<float>() * walkSpeed;
    }

    public void OnJump()
    {
        // Buffer jump due to desync between input and fixed update
        wantsToJump = true;
    }

    private void MoveHorizontal()
    {
        float horizontalMove = velocity.x * Time.fixedDeltaTime;

        // Only attempt move if there is some input
        if (horizontalMove != 0.0f)
        {
            // Raycast against "World" objects
            float horizontalDirection = horizontalMove < 0.0f ? -1.0f : 1.0f;
            RaycastHit2D hitResult = Physics2D.BoxCast(currentPosition, characterCollider.size, 0.0f, new Vector2(horizontalDirection, 0.0f), Mathf.Abs(horizontalMove), LayerMask.GetMask("World"));

            // If we hit something, we can only move the distance of the raycast, minus the minimum separation (to prevent getting stuck in walls)
            if (hitResult)
            {
                horizontalMove = (hitResult.distance - minimumSeparation) * horizontalDirection;
            }

            // Apply movement
            currentPosition.x += horizontalMove;
        }
    }

    private void MoveVertical()
    {
        // Reset grounded state
        isGrounded = false;

        // Test for ground just in case we're hovering above it
        RaycastHit2D groundResult = Physics2D.BoxCast(currentPosition, characterCollider.size, 0.0f, new Vector2(0.0f, -1.0f), groundDistance, LayerMask.GetMask("World"));

        if (groundResult)
        {
            // Limit ground downward velocity to 0 - still allows for jumps
            isGrounded = true;
            velocity.y = Mathf.Max(velocity.y, 0.0f);
        }
        else
        {
            // Apply gravitational acceleration
            velocity.y -= gravityAcceleration * Time.fixedDeltaTime;
            velocity.y = Mathf.Min(velocity.y, terminalVelocity);
        }

        // Check for buffered jump
        if (wantsToJump)
        {
            // Only jump if we're grounded
            if (isGrounded)
            {
                velocity.y = jumpVelocity;
            }

            // Reset buffered jump
            wantsToJump = false;
        }

        // Don't do checks unless we're moving
        if (velocity.y != 0.0f)
        {
            float verticalMove = velocity.y * Time.fixedDeltaTime;
            float verticalDirection = velocity.y < 0.0f ? -1.0f : 1.0f;

            // Raycast against "World" objects
            RaycastHit2D hitResult = Physics2D.BoxCast(currentPosition, characterCollider.size, 0.0f, new Vector2(0.0f, verticalDirection), Mathf.Abs(verticalMove), LayerMask.GetMask("World"));

            // If we hit something, we can only move the distance of the raycast, minus the minimum separation (to prevent getting stuck in walls)
            if (hitResult)
            {
                verticalMove = (hitResult.distance - minimumSeparation) * verticalDirection;

                // Set grounded if we hit something below us
                if (verticalDirection <= 0.0f)
                {
                    // Limit velocity on landing
                    velocity.y = Mathf.Max(0.0f, velocity.y);
                }
            }

            // Actually apply the movement
            currentPosition.y += verticalMove;
        }
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

    void OnRepair()
    {
        Collider2D hitCollider = Physics2D.OverlapBox(gameObject.transform.position, transform.localScale, 0, m_LayerMask);
        //Check when there is a new collider coming into contact with the box
        //Output all of the collider names
        if(hitCollider != null){
            GameObject device = hitCollider.gameObject;
            device.GetComponent<Device>().interact();
        }
    }

    public void setCurrentPosition(Vector2 newPosition){
        currentPosition = newPosition;
    }
}
