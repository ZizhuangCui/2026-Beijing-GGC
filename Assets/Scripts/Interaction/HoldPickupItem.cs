using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HoldPickupItem : MonoBehaviour
{
    public Vector2 localOffset = new Vector2(0.3f, 0.1f);
    public string name;
    public int id;
    private Rigidbody2D rb;
    private Collider2D col;
    private bool isHeld;
    private Transform followPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        if (rb != null)
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    public bool IsHeld => isHeld;

    public void BeginHold(Transform handPoint)
    {
        if (isHeld) return;

        followPoint = handPoint;
        isHeld = true;

        if (col != null) col.isTrigger = true;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // 立刻对齐一次
        Vector2 p = (Vector2)followPoint.TransformPoint(localOffset);
        if (rb != null) rb.position = p;
        else transform.position = new Vector3(p.x, p.y, 0f);
    }

    public void EndHold()
    {
        if (!isHeld) return;

        isHeld = false;
        followPoint = null;

        if (col != null) col.isTrigger = false;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (!isHeld || followPoint == null) return;

        Vector2 target = (Vector2)followPoint.TransformPoint(localOffset);

        if (rb != null)
            rb.MovePosition(target);          // ✅ 关键：用物理方式移动，不闪
        else
            transform.position = new Vector3(target.x, target.y, 0f);
    }
}