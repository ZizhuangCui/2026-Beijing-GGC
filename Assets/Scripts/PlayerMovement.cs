using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public PlayerMovementStats playerMovementStats;
    [SerializeField] private Collider2D feetCollider;
    [SerializeField] private Collider2D bodyCollider;

    private Rigidbody2D rigidbody2d;
    private Vector2 moveVelocity;
    private bool isFacingRight;

    //collision checks
    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    private bool isGrounded;
    private bool isHeadBumped;

    //jump
    public float verticalVelocity {  get; private set; }
    private bool isJumping;
    private bool isFastFalling;
    private bool isFalling;
    private float fastFallTime;
    private float fastFallReleaseSpeed;
    private int numberOfJumpUsed;

    //Apex
    private float apexPoint;
    private float timePastApexThreshold;
    private bool isPastApexThreshold;

    //Jump buffer
    private float jumpBufferTimer;
    private bool jumpReleasedDuringBuffer;

    //Coyote time
    private float coyoteTimer;

    private Animator animator;

    //Sound Effects
    [SerializeField] private AudioSource[] moveSounds;
    [SerializeField] private AudioSource[] jumpSounds;
    [SerializeField] private AudioSource[] landSounds;
    [SerializeField] private AudioSource secondJumpSound;

    private void Awake()
    {
        isFacingRight = true;

        rigidbody2d = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CountTimers();
        JumpChecks();
        
        animator.SetFloat("velocity_x", Mathf.Abs(rigidbody2d.velocity.x));
        animator.SetFloat("velocity_y", rigidbody2d.velocity.y);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();

        if (!isGrounded)
        {
            Move(playerMovementStats.groundAcceleartion, playerMovementStats.gronndDeceleration, InputManager.Movement);
        }
        else
        {
            Move(playerMovementStats.airAcceleration, playerMovementStats.airDeceleration, InputManager.Movement);
        }
    }

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);

            Vector2 targetVelocity = new Vector2(moveInput.x, 0.0f) * playerMovementStats.maxMoveSpeed;
            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else if(moveInput == Vector2.zero)
        {
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        rigidbody2d.velocity = new Vector2(moveVelocity.x, rigidbody2d.velocity.y);
    }

    public void StopMoving()
    {
        moveVelocity = Vector2.zero;
    }

    public void PlayMoveSound()
    {
        foreach (AudioSource s in moveSounds)
        {
            if (s.isPlaying)
            {
                s.Stop();
            }
        }
        float rand4 = Random.Range(0, 100);
        if (rand4 < 100 / 4)
        {
            moveSounds[0].Play();
            Debug.Log("Play walk1");
        }
        else if (rand4 >= 100 / 4 && rand4 < 200 / 4)
        {
            moveSounds[1].Play();
            Debug.Log("Play walk2");
        }
        else if (rand4 >= 200 / 4 && rand4 < 300 / 4)
        {
            moveSounds[2].Play();
            Debug.Log("Play walk3");
        }
        else
        {
            moveSounds[3].Play();
            Debug.Log("Play walk4");
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if(isFacingRight && moveInput.x<0)
        {
            Turn(false);
        }
        else if(!isFacingRight && moveInput.x>0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if(turnRight)
        {
            isFacingRight = true;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        else
        {
            isFacingRight = false;
            transform.Rotate(0.0f, -180.0f, 0.0f);
        }
    }

    private void GroundCheck()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollider.bounds.center.x, feetCollider.bounds.min.y);
        Vector2 boxCastSize = new Vector2(feetCollider.bounds.size.x, playerMovementStats.groundDetectionRayLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0.0f, Vector2.down, playerMovementStats.groundDetectionRayLength, playerMovementStats.groundLayer);

        if(groundHit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void BumpedHeadCheck()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollider.bounds.center.x, bodyCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(feetCollider.bounds.size.x * playerMovementStats.headWidth, playerMovementStats.headDetectionRayLength);

        headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0.0f, Vector2.up, playerMovementStats.headDetectionRayLength, playerMovementStats.groundLayer);

        if (headHit.collider != null)
        {
            isHeadBumped = true;
        }
        else
        {
            isHeadBumped = false;
        }
    }
    private void CollisionChecks()
    {
        GroundCheck();
        BumpedHeadCheck();
    }

    private void JumpChecks()
    {
        if (InputManager.JumpWasPressed)
        {
            jumpBufferTimer = playerMovementStats.jumpBufferTime;
            jumpReleasedDuringBuffer = false;
        }

        if (InputManager.JumpWasReleased)
        {
            if(jumpBufferTimer > 0)
            {
                jumpReleasedDuringBuffer = true;
            }

            if(isJumping && verticalVelocity>0)
            {
                if(isPastApexThreshold)
                {
                    isPastApexThreshold = false;
                    isFastFalling = true;
                    fastFallTime = playerMovementStats.timeForUpwardsCancel;
                    verticalVelocity = 0;
                }
                else
                {
                    isFastFalling = true;
                    fastFallReleaseSpeed = verticalVelocity;
                }
            }
        }

        if(jumpBufferTimer>0 && !isJumping && (isGrounded || coyoteTimer>0))
        {
            InitialJump(1);
            float rand3 = Random.Range(0, 100);
            if (rand3 < 100/3)
            {
                jumpSounds[0].Play();
                Debug.Log("play jump1");
            }
            else if (rand3>=100/3 && rand3< 200/3)
            {
                jumpSounds[1].Play();
                Debug.Log("play jump2");
            }
            else
            {
                jumpSounds[2].Play();
                Debug.Log("play jump3");
            }

            if (jumpReleasedDuringBuffer)
            {
                isFastFalling = true;
                fastFallReleaseSpeed = verticalVelocity;
            }
        }
        else if(jumpBufferTimer>0f && isJumping && numberOfJumpUsed<playerMovementStats.numberOfJumpAllowed)
        {
            isFastFalling = false;
            InitialJump(1);
            secondJumpSound.Play();
            Debug.Log("play second jump");
        }

        //landed
        if((isJumping || isFalling) && isGrounded && verticalVelocity <=0)
        {
            isJumping = false;
            isFalling = false;
            isFastFalling = false;
            fastFallTime = 0.0f;
            isPastApexThreshold = false;
            numberOfJumpUsed = 0;
            verticalVelocity = Physics2D.gravity.y;
            float rand2 = Random.Range(0, 100);
            if (rand2 < 50)
            {
                landSounds[0].Play();
                Debug.Log("play land1");
            }
            else
            {
                landSounds[1].Play();
                Debug.Log("play land2");
            }
        }
    }

    private void InitialJump(int numOfJumpUsed)
    {
        foreach (AudioSource s in jumpSounds)
        {
            if(s.isPlaying)
            {
                s.Stop();
            }
        }
        if (!isJumping)
        {
            isJumping = true;
        }

        jumpBufferTimer = 0.0f;
        numberOfJumpUsed += numOfJumpUsed;
        verticalVelocity = playerMovementStats.initialJumpVelocity;
    }

    private void Jump()
    {
        if(isJumping)
        {
            if(isHeadBumped)
            {
                isFastFalling = true;
            }

            if(verticalVelocity >=0)
            {
                apexPoint = Mathf.InverseLerp(playerMovementStats.initialJumpVelocity, 0.0f, verticalVelocity);
                if (apexPoint > playerMovementStats.apexThreshold)
                {
                    if (!isPastApexThreshold)
                    {
                        isPastApexThreshold = true;
                        timePastApexThreshold = 0.0f;
                    }
                    else
                    {
                        timePastApexThreshold += Time.fixedDeltaTime;
                        if (timePastApexThreshold < playerMovementStats.apexHangTime)
                        {
                            verticalVelocity = 0.0f;
                        }
                        else
                        {
                            verticalVelocity = -0.01f;
                        }
                    }
                }
                else
                {
                    verticalVelocity += playerMovementStats.gravity * Time.fixedDeltaTime;
                    if (isPastApexThreshold)
                    {
                        isPastApexThreshold = false;
                    }
                }
            }
            else if(!isFastFalling)
            {
                verticalVelocity += playerMovementStats.gravity * playerMovementStats.gravityOnRelaeseMultiplier * Time.fixedDeltaTime;
            }
            else if(verticalVelocity<0)
            {
                if(!isFalling)
                {
                    isFalling = true;
                }
            }
        }
        //jump cut
        if(isFastFalling)
        {
            if(fastFallTime>=playerMovementStats.timeForUpwardsCancel)
            {
                verticalVelocity += playerMovementStats.gravity * playerMovementStats.gravityOnRelaeseMultiplier * Time.fixedDeltaTime;
            }
            else
            {
                verticalVelocity = Mathf.Lerp(fastFallReleaseSpeed, 0.0f, (fastFallTime/playerMovementStats.timeForUpwardsCancel));
            }

            fastFallTime += Time.fixedDeltaTime;
        }
        //falling with normal gravity
        if(!isGrounded && !isJumping)
        {
            if(!isFalling)
            {
                isFalling = true;
            }
            verticalVelocity += playerMovementStats.gravity * Time.fixedDeltaTime;
        }

        verticalVelocity = Mathf.Clamp(verticalVelocity, -playerMovementStats.maxFallSpeed, 50f);

        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, verticalVelocity);
    }
    
    private void CountTimers()
    {
        jumpBufferTimer -= Time.deltaTime;

        if(!isGrounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = playerMovementStats.jumpCoyoteTime;
        }
    }

    public void EnableDoubleJump()
    {
        playerMovementStats.numberOfJumpAllowed = 2;
    }

    public void DisableDoubleJump()
    {
        playerMovementStats.numberOfJumpAllowed = 1;
    }
}
