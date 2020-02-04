using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    // Horizontal movement properties
    public float walkSpeed = 10.0f;
    public float walkAcceleration = 40.0f;
    public float walkFriction = 44.0f;
    public bool canAccelerateFloating = false;
    public float airAccelerationFactor = 0.7f;

    // Vertical movement properties
    public float gravityAcceleration = 12.0f;
    public float terminalVelocity = 9.0f;
    public float jumpVelocity = 4.0f;
    public float groundDistance = 0.02f;
    public float jumpHoldAcceleration = 7.0f;
    public float jumpHoldTime = 0.2f;

    // Physics properties
    public float minimumSeparation = 0.01f;
    public bool doPositionSmoothing = false;

    // Components
    private BoxCollider2D characterCollider;
    public Slider repairSlider;
    public TextMeshProUGUI cooldownText;

    // Private physics
    private Vector2 velocity = new Vector2(0.0f, 0.0f);
    private bool isGrounded = false;
    private bool isJumpHeld = false;
    private float currentJumpHeldTime = 0.0f;

    // Public physics
    public LayerMask m_LayerMask;

    // Interpolation
    private Vector2 previousPosition;
    private Vector2 currentPosition;
    private float previousTime;

    // Input buffer
    private bool wantsToJump = false;
    private float horizontalAxis = 0.0f;

    // Game Logic
    public int location = 0;
    public bool isStunned = false;
    public double lastDamaged = Constants.TIME_STUNNED;
    bool repairing = false;
    private double time = 0;
    public double stunTimeRemaining = Constants.TIME_STUNNED;
    public double stunCooldownCounter = 0;
    public int stunCooldownDisplay;
    private Device targetDevice;
    public GameState.PlayerState playerState;
    private bool isAttacking = false;

    // Attack
    public GameObject attackHitbox;
    private Collider2D attackCollider;
    public GameObject stunEffect;

    void Awake()
    {
        characterCollider = GetComponent<BoxCollider2D>();
        attackCollider = attackHitbox.GetComponent<BoxCollider2D>();
    }

    void Start() {
        repairSlider = GetComponentInChildren<Slider>();
        repairSlider.gameObject.SetActive(false);
        cooldownText = GetComponentInChildren<TextMeshProUGUI>();

        if(!cooldownText)
        {
            Debug.Log("can't find cooldown");
        }
    }

    private void Update()
    {
        CheckStun();
        CheckRepair();
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

    public void OnHorizontal(InputValue value)
    {
        horizontalAxis = value.Get<float>();
    }

    public void OnJump()
    {
        // Buffer jump due to desync between input and fixed update
        wantsToJump = true;
    }

    public void OnJumpRelease()
    {
        isJumpHeld = false;
    }

    private void MoveHorizontal()
    {
        float desiredHorizontalVelocity = 0.0f;

        // Check to see if we can move
        if (GetCanMove())
        {
            desiredHorizontalVelocity = horizontalAxis * walkSpeed;
        }

        float accelerationFactor = 1.0f;

        // Set acceleration factor differently when we're in the air
        if (!isGrounded)
        {
            if (canAccelerateFloating)
            {
                accelerationFactor = airAccelerationFactor;
            }
            else
            {
                // Don't accelerate if disabled
                accelerationFactor = 0.0f;
            }
        }

        float horizontalAcceleration;

        if (Mathf.Sign(desiredHorizontalVelocity) == Mathf.Sign(velocity.x))
        {
            horizontalAcceleration = walkAcceleration;
        }
        // Use different acceleration if we're changing direction/stopping
        else
        {
            horizontalAcceleration = walkFriction;
        }

        float deltaHorizontalVelocityLimit = horizontalAcceleration * accelerationFactor * Time.fixedDeltaTime;

        // Move towards desired velocity, but limit by acceleration
        float deltaHorizontalVelocity = Mathf.Clamp(desiredHorizontalVelocity - velocity.x, -deltaHorizontalVelocityLimit, deltaHorizontalVelocityLimit);
        velocity.x += deltaHorizontalVelocity;

        float horizontalMove = velocity.x * Time.fixedDeltaTime;

        // Only attempt move if there is some input
        if (horizontalMove != 0.0f)
        {
            GetComponent<Animator>().SetFloat("horizontalSpeed", velocity.x);

            // Raycast against "World" objects
            float horizontalDirection = Mathf.Sign(horizontalMove);
            RaycastHit2D hitResult = Physics2D.BoxCast(currentPosition, characterCollider.size, 0.0f, new Vector2(horizontalDirection, 0.0f), Mathf.Abs(horizontalMove), LayerMask.GetMask("World"));

            // If we hit something, we can only move the distance of the raycast, minus the minimum separation (to prevent getting stuck in walls)
            if (hitResult)
            {
                horizontalMove = (hitResult.distance - minimumSeparation) * horizontalDirection;

                if (horizontalDirection == Mathf.Sign(velocity.x))
                {
                    velocity.x = 0.0f;
                }
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

            // Check jump held
            if (isJumpHeld)
            {
                // Accumulate time
                currentJumpHeldTime += Time.fixedDeltaTime;

                // If we haven't held for the max time, apply some upward force
                if (currentJumpHeldTime <= jumpHoldTime)
                {
                    velocity.y += jumpHoldAcceleration * Time.fixedDeltaTime;
                }
                // Otherwise reset the jump held
                else
                {
                    isJumpHeld = false;
                }
            }
        }

        // Check for buffered jump
        if (wantsToJump)
        {
            // Only jump if we're grounded
            if (isGrounded && GetCanMove())
            {
                velocity.y = jumpVelocity;

                // Start holding the jump
                isJumpHeld = true;
                currentJumpHeldTime = 0.0f;
            }

            // Reset buffered jump
            wantsToJump = false;
        }

        // Don't do checks unless we're moving
        if (velocity.y != 0.0f)
        {
            float verticalMove = velocity.y * Time.fixedDeltaTime;
            float verticalDirection = Mathf.Sign(velocity.y);

            GetComponent<Animator>().SetFloat("verticalSpeed", velocity.y);

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
                else if (verticalDirection > 0.0f)
                {
                    // Limit velocity on hit
                    velocity.y = Mathf.Min(0.0f, velocity.y);

                    // Force stop jump holding
                    isJumpHeld = false;
                }
            }

            // Actually apply the movement
            currentPosition.y += verticalMove;
        }
        GetComponent<Animator>().SetBool("isGrounded", isGrounded);
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
        if (GetCanMove()) {
            Collider2D hitCollider = Physics2D.OverlapBox(gameObject.transform.position, transform.localScale, 0, m_LayerMask);
            if (hitCollider != null)
            {
                Device hitDevice = hitCollider.GetComponentInParent<Device>();

                if (hitDevice && hitDevice.isDamaged)
                {
                    targetDevice = hitDevice;
                    repairSlider.gameObject.SetActive(true);
                    repairing = true;
                    time = 0.0f;
                }
            }
        }
    }

    // check whether stunned or can stun
    private void CheckStun()
    {
        if (isStunned)
        {
            stunTimeRemaining -= Time.deltaTime;

            if (stunTimeRemaining <= 0.0f)
            {
                isStunned = false;
                GetComponent<Animator>().SetBool("isStunned", false);
                stunEffect.SetActive(false);
            }
        }
        if (stunCooldownCounter > 0)
        {
            cooldownText.gameObject.SetActive(true);
            stunCooldownCounter -= Time.deltaTime;
            if(stunCooldownCounter < 0)
            {
                stunCooldownCounter = 0;
            }
            stunCooldownDisplay = (int)stunCooldownCounter + 1;
            cooldownText.text = stunCooldownDisplay.ToString();
        }
        else
        {
            cooldownText.gameObject.SetActive(false);
        }
    }

    private void CheckRepair()
    {
        if (repairing)
        {
            time += Time.deltaTime;

            float timerProgress = (float)time / 1.5f;
            repairSlider.value = timerProgress;

            if (time >= 1.5f)
            {
                FinishRepair(true);
            }
        }
    }

    private void FinishRepair(bool wasSuccessful)
    {
        repairing = false;
        repairSlider.gameObject.SetActive(false);

        if (wasSuccessful)
        {
            playerState.score += targetDevice.interact();
        }
    }

   public void OnAttack()
    {
        if (GetCanMove())
        {
            if(stunCooldownCounter == 0)
            {
                isAttacking = true;
                GetComponent<Animator>().SetBool("isAttacking", isAttacking);

                stunCooldownCounter = Constants.STUN_COOLDOWN;
            }
        }
    }

    public void OnAttackAnimationActive()
    {
        StunOtherPlayers();
    }

    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
        GetComponent<Animator>().SetBool("isAttacking", isAttacking);
    }

    private void StunOtherPlayers()
    {
        if (GetComponent<SpriteRenderer>().flipX)
        {
            attackHitbox.transform.localScale = new Vector3(-1.0f, 1.0f);
        }
        else
        {
            attackHitbox.transform.localScale = new Vector3(1.0f, 1.0f);
        }

        // Set a contact filter for the layer mask
        ContactFilter2D characterFilter = new ContactFilter2D();
        characterFilter.SetLayerMask(LayerMask.GetMask("Character"));
        characterFilter.useLayerMask = true;

        // Get all colliders on layer
        List<Collider2D> characterOverlaps = new List<Collider2D>();
        attackCollider.OverlapCollider(characterFilter, characterOverlaps);

        // Check all overlapping character colliders
        foreach (Collider2D overlap in characterOverlaps)
        {
            
            // Try to grab the player
            Player overlapPlayer = overlap.GetComponentInParent<Player>();

            // Ignore this player
            if (overlapPlayer && overlapPlayer != this && !overlapPlayer.isStunned)
            {
                overlapPlayer.Stun();
            }
        }
    }

    public void Stun()
    {
        stunTimeRemaining = Constants.TIME_STUNNED;
        isStunned = true;
        OnAttackAnimationEnd();
        GetComponent<Animator>().SetBool("isStunned", true);


        FinishRepair(false);

        stunEffect.SetActive(true);
    }

    public void setCurrentPosition(Vector2 newPosition){
        currentPosition = newPosition;
    }

    private bool GetCanMove()
    {
        if (isStunned || repairing)
        {
            return false;
        }

        return true;
    }
 
}
