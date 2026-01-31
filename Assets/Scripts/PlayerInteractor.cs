using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Key")]
    public KeyCode interactKey = KeyCode.F;

    [Header("Detection")]
    [SerializeField] private Collider2D interactTrigger; // 一个 IsTrigger 的范围Collider

    private readonly List<IInteractable> inRange = new();

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            var target = PickBest();
            if (target != null)
                target.Interact(gameObject);
        }
    }

    private IInteractable PickBest()
    {
        if (inRange.Count == 0) return null;

        IInteractable best = null;
        int bestPriority = int.MinValue;
        float bestDist = float.MaxValue;

        Vector3 p = transform.position;

        for (int i = inRange.Count - 1; i >= 0; i--)
        {
            var it = inRange[i];
            if (it == null) { inRange.RemoveAt(i); continue; }

            // 距离作为次级排序
            var mono = it as MonoBehaviour;
            float d = mono != null ? Vector2.Distance(p, mono.transform.position) : 9999f;

            if (it.Priority > bestPriority || (it.Priority == bestPriority && d < bestDist))
            {
                best = it;
                bestPriority = it.Priority;
                bestDist = d;
            }
        }

        return best;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var it))
        {
            if (!inRange.Contains(it)) inRange.Add(it);
            // TODO: 这里可以显示UI提示：it.Prompt + " (F)"
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out var it))
        {
            inRange.Remove(it);
            // TODO: 若离开后列表为空，隐藏UI提示
        }
    }
}
