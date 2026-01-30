using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : Enemy
{
    // 基础参数
    [Header("Movement Settings")]
    [SerializeField] private float patrolRange = 5f;
    [SerializeField] private float detectionRange = 3f;
    [SerializeField] private Transform player;


    // 状态枚举
    private enum AIState { Patrolling, Chasing, Returning }
    private AIState currentState = AIState.Patrolling;

    // 内部变量
    private Vector2 startPosition;
    private bool movingRight = true;
    private Animator animator;

    #region Unity生命周期
    void Start() {
        InitializeReferences();
        CacheStartPosition();
        FindPlayerIfNull();
    }

    void Update() {
        if (player == null) return;

        UpdateStateMachine();
        UpdateAnimations();
    }

    void FixedUpdate() {
        HandleMovement();
    }

    void OnDrawGizmosSelected() {
        DrawMovementGizmos();
    }
    #endregion

    #region 初始化
    private void InitializeReferences() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void CacheStartPosition() {
        startPosition = transform.position;
    }

    private void FindPlayerIfNull() {
        if (player == null) {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj) player = playerObj.transform;
        }
    }
    #endregion

    #region 状态机
    private void UpdateStateMachine() {
        float playerDistanceSqr = (transform.position - player.position).sqrMagnitude;
        float startDistanceSqr = (rb.position - startPosition).sqrMagnitude;

        // 状态转换逻辑
        if (playerDistanceSqr <= detectionRange * detectionRange) {
            currentState = AIState.Chasing;
        }
        else if (currentState == AIState.Chasing && startDistanceSqr > (detectionRange * 1.5f).Squared()) {
            currentState = AIState.Returning;
        }
        else if (currentState == AIState.Returning && startDistanceSqr < 0.1f * 0.1f) {
            currentState = AIState.Patrolling;
            movingRight = true;
            UpdateFacingDirection();
        }
    }
    #endregion

    #region 移动控制
    private void HandleMovement() {
        switch (currentState) {
            case AIState.Patrolling:
                PatrolMovement();
                break;
            case AIState.Chasing:
                ChaseMovement();
                break;
            case AIState.Returning:
                ReturnMovement();
                break;
        }
    }

    private void PatrolMovement() {
        float targetX = startPosition.x + (movingRight ? patrolRange : -patrolRange);
        Vector2 targetPosition = new Vector2(targetX, startPosition.y);

        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            targetPosition,
            normalSpeed * Time.fixedDeltaTime
        );
        rb.MovePosition(newPos);

        // 到达边界时转向
        if (Mathf.Abs(rb.position.x - targetPosition.x) < 0.1f) {
            movingRight = !movingRight;
            UpdateFacingDirection();
        }
    }

    private void ChaseMovement() {
        Vector2 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        rb.velocity = new Vector2(direction.x * chaseSpeed, rb.velocity.y);

        // 实时调整朝向
        if (direction.x != 0) {
            bool shouldFlip = (direction.x > 0) != (transform.localScale.x > 0);
            if (shouldFlip) Flip();
        }
    }

    private void ReturnMovement() {
        Vector2 direction = (startPosition - rb.position).normalized;
        rb.velocity = direction * normalSpeed;

        // 接近起点时重置状态
        if ((rb.position - startPosition).sqrMagnitude < 0.1f * 0.1f) {
            rb.position = startPosition;
            currentState = AIState.Patrolling;
        }
    }
    #endregion

    #region 辅助功能
    private void UpdateAnimations() {
        bool isMoving = rb.velocity.sqrMagnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isChasing", currentState == AIState.Chasing);
    }

    private void Flip() {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void UpdateFacingDirection() {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (movingRight ? 1 : -1);
        transform.localScale = scale;
    }

    private void DrawMovementGizmos() {
        // 绘制巡逻范围
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(startPosition, new Vector3(patrolRange * 2, 0.5f, 0));

        // 绘制检测范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
    #endregion
}

// 扩展方法类
public static class FloatExtensions {
    public static float Squared(this float value) {
        return value * value;
    }
}
