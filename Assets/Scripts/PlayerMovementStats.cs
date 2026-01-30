using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Player Movement")]
public class PlayerMovementStats : ScriptableObject
{
    public int currentLevel = 1;

    [Header("Move")]
    [Range(1.0f, 100f)] public float maxMoveSpeed = 10.0f;
    [Range(0.25f, 50f)] public float groundAcceleartion = 5.0f;
    [Range(0.25f, 200f)] public float gronndDeceleration = 20.0f;
    [Range(0.25f, 50f)] public float airAcceleration = 5.0f;
    [Range(0.25f, 50f)] public float airDeceleration = 5.0f;

    [Header("Grounded/Collision Detection")]
    public LayerMask groundLayer;
    public float groundDetectionRayLength = 0.02f;
    public float headDetectionRayLength = 0.02f;
    [Range(0.0f, 1.0f)] public float headWidth = 0.75f;

    [Header("Jump")]
    public float jumpHeight = 5.0f;
    [Range(1.0f, 1.1f)] public float jumpHeightCompensationFactor = 1.054f;
    public float timeTillJumpApex = 0.35f;
    [Range(0.01f, 5.0f)] public float gravityOnRelaeseMultiplier = 2.0f;
    public float maxFallSpeed = 26.0f;
    [Range(1, 5)] public int numberOfJumpAllowed = 1;

    [Header("Jump Cut")]
    [Range(0.02f, 0.3f)] public float timeForUpwardsCancel = 0.027f;

    [Header("Jump Apex")]
    [Range(0.5f, 1.0f)] public float apexThreshold = 0.97f;
    [Range(0.01f, 1.0f)] public float apexHangTime = 0.075f;

    [Header("Jump Buffer")]
    [Range(0.0f, 1.0f)] public float jumpBufferTime = 0.15f;

    [Header("Jump Coyote Time")]
    [Range(0.0f, 1.0f)] public float jumpCoyoteTime = 0.1f;

    public float gravity { get; private set; }

    public float initialJumpVelocity { get; private set; }

    public float adjustedJumpHeight { get; private set; }

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        adjustedJumpHeight = jumpHeight * jumpHeightCompensationFactor;
        gravity = -(2.0f * jumpHeight) / Mathf.Pow(timeTillJumpApex, 2.0f);
        initialJumpVelocity = Mathf.Abs(gravity) * timeTillJumpApex;
    }

}
