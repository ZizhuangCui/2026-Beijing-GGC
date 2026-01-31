using System.Collections.Generic;
using UnityEngine;

public class HoldPickupController : MonoBehaviour
{
    [Header("Input")]
    public KeyCode holdKey = KeyCode.F;

    [Header("Detection")]
    [SerializeField] private Collider2D holdTrigger; // 一个 IsTrigger 的范围Collider

    [Header("Release")]
    public bool inheritPlayerVelocity = true;
    public float extraReleaseUp = 0f; // 需要“松开弹一下”可用

    private readonly List<HoldPickupItem> candidates = new();
    private HoldPickupItem heldItem;

    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(holdKey))
        {
            TryBeginHold();
        }

        if (Input.GetKeyUp(holdKey))
        {
            EndHold();
        }

        // 可选：按住期间，如果物体被销毁/离开，自动释放
        if (heldItem != null && !heldItem.IsHeld)
            heldItem = null;
    }

    private void TryBeginHold()
    {
        if (heldItem != null) return;

        var best = PickNearestHoldable();
        if (best == null) return;
        if (!best.CanBeHeld()) return;

        best.BeginHold(transform);
        heldItem = best;
    }

    private void EndHold()
    {
        if (heldItem == null) return;

        Vector2 releaseVel = Vector2.zero;
        if (inheritPlayerVelocity && playerRb != null)
            releaseVel = playerRb.velocity;

        releaseVel += Vector2.up * extraReleaseUp;

        heldItem.EndHold(releaseVel);
        heldItem = null;
    }

    private HoldPickupItem PickNearestHoldable()
    {
        if (candidates.Count == 0) return null;

        HoldPickupItem best = null;
        float bestDist = float.MaxValue;
        Vector3 p = transform.position;

        for (int i = candidates.Count - 1; i >= 0; i--)
        {
            var it = candidates[i];
            if (it == null) { candidates.RemoveAt(i); continue; }

            float d = Vector2.Distance(p, it.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                best = it;
            }
        }

        return best;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<HoldPickupItem>(out var item))
        {
            if (!candidates.Contains(item))
                candidates.Add(item);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<HoldPickupItem>(out var item))
        {
            candidates.Remove(item);

            // 如果你“离开范围就强制掉落”，打开下面
            // if (heldItem == item) EndHold();
        }
    }
}
