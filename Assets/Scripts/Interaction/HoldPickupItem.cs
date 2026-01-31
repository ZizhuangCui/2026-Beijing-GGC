using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HoldPickupItem : MonoBehaviour
{
    public Vector2 localOffset = new Vector2(0.3f, 0.1f);

    private Rigidbody2D rb;
    private bool isHeld;
    private Transform followPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool IsHeld => isHeld;

    public void BeginHold(Transform handPoint)
    {
        followPoint = handPoint;
        isHeld = true;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = true;   // 抓起：关物理
            rb.simulated = true;
        }

        transform.position = followPoint.TransformPoint(localOffset);
    }

    public void EndHold()
    {
        isHeld = false;
        followPoint = null;

        if (rb != null)
        {
            rb.isKinematic = false;  // 松开：开物理 -> 会掉地上
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = true;
        }
    }

    private void LateUpdate()
    {
        if (!isHeld || followPoint == null) return;
        transform.position = followPoint.TransformPoint(localOffset);
    }
}
