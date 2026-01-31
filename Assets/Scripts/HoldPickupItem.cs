using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HoldPickupItem : MonoBehaviour
{
    [Header("Hold Behavior")]
    public bool followHolder = true;
    public Vector2 holdOffset = new Vector2(0.6f, 0.2f); // 物体相对玩家的位置
    public float followLerp = 25f;

    private Rigidbody2D rb;
    private Collider2D col;

    private Transform holder;
    private Vector3 targetPos;
    private bool isHeld;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public bool IsHeld => isHeld;

    public bool CanBeHeld()
    {
        // 你可以加条件：比如太重、锁住、正在动画等
        return true;
    }

    public void BeginHold(Transform newHolder)
    {
        holder = newHolder;
        isHeld = true;

        // 如果物体有刚体：抓起时不让物理干扰
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;
        }

        // 可选：抓起时避免和玩家卡住
        // col.enabled = false;
    }

    public void EndHold(Vector2 releaseVelocity)
    {
        isHeld = false;

        // 可选：放下时恢复碰撞
        // col.enabled = true;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.velocity = releaseVelocity; // 可给一点继承速度/抛掷
        }

        holder = null;
    }

    private void LateUpdate()
    {
        if (!isHeld || holder == null) return;
        if (!followHolder) return;

        // 跟随玩家位置（简单稳定）
        targetPos = holder.position + (Vector3)holdOffset;
        transform.position = Vector3.Lerp(transform.position, targetPos, followLerp * Time.deltaTime);
    }
}
