using UnityEngine;

public class HoldPickupController : MonoBehaviour
{
    public KeyCode holdKey = KeyCode.F;

    [Header("Pick detection")]
    public float pickupRadius = 0.6f;
    public LayerMask pickupLayer;

    [Header("Hold")]
    public Transform handPoint;

    private HoldPickupItem held;

    private void Update()
    {
        bool holdingKey = Input.GetKey(holdKey);

        if (holdingKey)
        {
            if (held == null)
                TryBeginHold();
        }
        else
        {
            if (held != null)
                EndHold();
        }
    }

    private void TryBeginHold()
    {
        if (handPoint == null)
        {
            Debug.LogError("HoldPickupController: handPoint not set.");
            return;
        }

        var target = FindNearestHoldable();
        if (target == null) return;

        target.BeginHold(handPoint);
        held = target;
    }

    private void EndHold()
    {
        held.EndHold();
        held = null;
    }

    private HoldPickupItem FindNearestHoldable()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, pickupRadius, pickupLayer);
        HoldPickupItem best = null;
        float bestDist = float.MaxValue;

        foreach (var h in hits)
        {
            if (!h.TryGetComponent(out HoldPickupItem item)) continue;
            if (item.IsHeld) continue;

            float d = Vector2.Distance(transform.position, item.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = item;
            }
        }

        return best;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
#endif
}
