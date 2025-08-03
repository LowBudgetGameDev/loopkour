using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walk")]
    [SerializeField][Range(1f, 100f)] private float maxWalkSpeed = 12.5f;
    [SerializeField][Range(0.25f, 50f)] private float groundAcceleration = 5f;
    [SerializeField][Range(0.25f, 50f)] private float groundDeceleration = 20f;
    [SerializeField][Range(0.25f, 50f)] private float airAcceleration = 5f;
    [SerializeField][Range(0.25f, 50f)] private float airDeceleration = 5f;

    [Header("Run")]
    [SerializeField][Range(1f, 100f)] private float maxRunSpeed = 20f;

    [Header("Grounded/Collision Checks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundDetectionRayLength = 0.02f;
    [SerializeField] private float headDetectionRayLength = 0.02f;
    [SerializeField][Range(0f, 1f)] private float headWidth = 0.75f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 6.5f;
    [SerializeField][Range(1f, 1.1f)] private float jumpHeightCompensationFactor = 1.054f;
    [SerializeField] private float timeTillJumpApex = 0.35f;
    [SerializeField][Range(0.01f, 5f)] private float gravityOnReleaseMultiplier = 2f;
    [SerializeField] private float maxFallSpeed = 26f;
    [SerializeField][Range(1, 5)] public int numberOfJumpsAllowed = 1;

    [Header("Jump Cut")]
    [SerializeField][Range(0.02f, 0.3f)] private float timeForUpwardsCancel = 0.027f;

    [Header("Jump Apex")]
    [SerializeField][Range(0.5f, 1f)] private float apexThreshold = 0.97f;
    [SerializeField][Range(0.01f, 1f)] private float apexHangTime = 0.075f;

    [Header("Jump Buffer")]
    [SerializeField][Range(0f, 1f)] private float jumpBufferTime = 0.125f;

    [Header("Jump Coyote Time")]
    [SerializeField][Range(0f, 1f)] private float jumpCoyoteTime = 0.1f;

    // Calculated Values
    private float gravity;
    private float initialJumpVelocity;
    private float adjustedJumpHeight;

    private void CalculateValues()
    {
        adjustedJumpHeight = jumpHeight * jumpHeightCompensationFactor;
        gravity = -(2f * adjustedJumpHeight) / Mathf.Pow(timeTillJumpApex, 2f);
        initialJumpVelocity = Mathf.Abs(gravity) * timeTillJumpApex;
    }

    [Space]

    [Header("References")]
    [SerializeField] private Collider2D feetCollider;
    [SerializeField] private Collider2D bodyCollider;

    // Controll Variables
    private bool runIsHeld;
    private Vector2 movement;
    private bool jumpWasPressed;
    private bool jumpWasReleased;

    private new Rigidbody2D rigidbody2D;

    // Movement Variables
    private Vector2 moveVelocity;
    private bool isFacingRight;

    // Collision Check Variables
    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    public bool isGrounded { get; private set; } // This is so that the animations can access this information
    private bool bumpedHead;

    // Jump Variables
    private float verticalVelocity;
    private bool isJumping;
    private bool isFastFalling;
    private bool isFalling;
    private float fastFallTime;
    private float fastFallReleaseSpeed;
    private int numberOfJumpsUsed;

    // Apex Variables
    private float apexPoint;
    private float timePastApexThreshold;
    private bool isPastApexThreshold;

    // Jump Buffer Variables
    private float jumpBufferTimer;
    private bool jumpReleasedDuringBuffer;

    // Coyote Time Variables
    private float coyoteTimer;

    private void Awake()
    {
        CalculateValues();

        isFacingRight = true;
        
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        runIsHeld = false; // No running in this game but im too lazy to remove it
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        jumpWasPressed = Input.GetKeyDown(KeyCode.Space);
        jumpWasReleased = Input.GetKeyUp(KeyCode.Space);

        CountTimers();
        JumpChecks();
        WalkSound();
    }

    private void FixedUpdate()
    {
        CollisionChecks();

        Jump();

        if (isGrounded)
        {
            Move(groundAcceleration, groundDeceleration, movement);
        }
        else
        {
            Move(airAcceleration, airDeceleration, movement);
        }
    }

    #region Movement

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput == Vector2.zero)
        {
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            rigidbody2D.linearVelocity = new Vector2(moveVelocity.x, rigidbody2D.linearVelocityY);
            return;
        }

        TurnCheck(moveInput);

        Vector2 targetVelocity = Vector2.zero;
        if (runIsHeld)
        {
            targetVelocity = new Vector2(moveInput.x, 0f) * maxRunSpeed;
        }
        else
        {
            targetVelocity = new Vector2(moveInput.x, 0f) * maxWalkSpeed;
        }

        moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        rigidbody2D.linearVelocity = new Vector2(moveVelocity.x, rigidbody2D.linearVelocityY);
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (isFacingRight && moveInput.x < 0)
        {
            Turn(false);
        }
        else if (!isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion

    #region Jump

    private void JumpChecks()
    {
        // when we press the jump button
        if (jumpWasPressed)
        {
            jumpBufferTimer = jumpBufferTime;
            jumpReleasedDuringBuffer = false;
        }

        // when we release the jump button
        if (jumpWasReleased)
        {
            if (jumpBufferTimer > 0f)
            {
                jumpReleasedDuringBuffer = true;
            }

            if (isJumping && verticalVelocity > 0f)
            {
                if (isPastApexThreshold)
                {
                    isPastApexThreshold = false;
                    isFastFalling = true;
                    fastFallTime = timeForUpwardsCancel;
                    verticalVelocity = 0f;
                }
                else
                {
                    isFastFalling = true;
                    fastFallReleaseSpeed = verticalVelocity;
                }
            }
        }

        // initiate jump with jump buffering and coyote time
        if (jumpBufferTimer > 0f && !isJumping && (isGrounded || coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (jumpReleasedDuringBuffer)
            {
                isFastFalling = true;
                fastFallReleaseSpeed = verticalVelocity;
            }
        }

        // double jump
        else if (jumpBufferTimer > 0f && isJumping && numberOfJumpsUsed < numberOfJumpsAllowed)
        {
            isFastFalling = false;
            InitiateJump(1);
        }

        // air jump after coyote time lapsed (take off an extra jump so we don't get a bonus jump)
        else if (jumpBufferTimer > 0f && isFalling && numberOfJumpsUsed < numberOfJumpsAllowed - 1)
        {
            InitiateJump(2);
            isFastFalling = false;
        }

        // landed
        if ((isJumping || isFalling) && isGrounded && verticalVelocity <= 0f)
        {
            isJumping = false;
            isFalling = false;
            isFastFalling = false;
            fastFallTime = 0f;
            isPastApexThreshold = false;
            numberOfJumpsUsed = 0;

            verticalVelocity = Physics2D.gravity.y;
        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!isJumping)
        {
            isJumping = true;
        }

        jumpBufferTimer = 0f;
        this.numberOfJumpsUsed += numberOfJumpsUsed;
        verticalVelocity = initialJumpVelocity;

        SoundManager.Instance.PlaySound(SoundManager.Sound.Jump);
    }

    private void Jump()
    {
        // apply gravity while jumping
        if (isJumping)
        {
            // check for head bump
            if (bumpedHead)
            {
                isFastFalling = true;
            }

            // gravity on ascending
            if (verticalVelocity >= 0f)
            {
                // apex controls
                apexPoint = Mathf.InverseLerp(initialJumpVelocity, 0f, verticalVelocity);

                if (apexPoint > apexThreshold)
                {
                    if (!isPastApexThreshold)
                    {
                        isPastApexThreshold = true;
                        timePastApexThreshold = 0f;
                    }

                    if (isPastApexThreshold)
                    {
                        timePastApexThreshold += Time.fixedDeltaTime;

                        if (timePastApexThreshold < apexHangTime)
                        {
                            verticalVelocity = 0f;
                        }
                        else
                        {
                            verticalVelocity = -0.01f;
                        }
                    }
                }

                // gravity on ascending but not past apex threshold
                else
                {
                    verticalVelocity += gravity * Time.fixedDeltaTime;

                    if (isPastApexThreshold)
                    {
                        isPastApexThreshold = false;
                    }
                }
            }

            // gravity on descending
            else if (!isFastFalling)
            {
                verticalVelocity += gravity * gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (verticalVelocity < 0f)
            {
                if (!isFalling)
                {
                    isFalling = true;
                }
            }
        }

        // jump cut
        if (isFastFalling)
        {
            if (fastFallTime >= timeForUpwardsCancel)
            {
                verticalVelocity += gravity * gravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else
            {
                verticalVelocity = Mathf.Lerp(fastFallReleaseSpeed, 0f, (fastFallTime / timeForUpwardsCancel));
            }

            fastFallTime += Time.fixedDeltaTime;
        }

        // normal gravity while falling
        if (!isGrounded && !isJumping)
        {
            if (!isFalling)
            {
                isFalling = true;
            }

            verticalVelocity += gravity * Time.fixedDeltaTime;
        }

        // clamp fall speed
        verticalVelocity = Mathf.Clamp(verticalVelocity, -maxFallSpeed, 50f);

        rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocityX, verticalVelocity);
    }

    #endregion

    #region Collision Checks

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollider.bounds.center.x, feetCollider.bounds.min.y);
        Vector2 boxCastSize = new Vector2(feetCollider.bounds.size.x, groundDetectionRayLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, groundDetectionRayLength, groundLayer);

        isGrounded = groundHit.collider != null;
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollider.bounds.center.x, bodyCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(feetCollider.bounds.size.x * headWidth, headDetectionRayLength);

        headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, headDetectionRayLength, groundLayer);

        bumpedHead = headHit.collider != null;
    }

    private void CollisionChecks()
    {
        IsGrounded();
        BumpedHead();
    }

    #endregion

    #region Timers

    private void CountTimers()
    {
        jumpBufferTimer -= Time.deltaTime;

        if (!isGrounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = jumpCoyoteTime;
        }
    }

    #endregion

    private float walkTimerMax = 0.25f;
    private float walkTimer;

    private void WalkSound()
    {
        if (!isGrounded || !(Mathf.Abs(rigidbody2D.linearVelocityX) > 0.1f)) return;

        walkTimer -= Time.deltaTime;

        if (walkTimer < 0f)
        {
            SoundManager.Instance.PlaySoundType(SoundManager.SoundType.Walk);
            walkTimer += walkTimerMax;
        }
    }
}
