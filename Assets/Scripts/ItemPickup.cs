using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName = "Coin"; // 物品名称
    public int value = 10; // 物品值（比如金币数量）
    private bool isCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected) // 确保 Player 有 "Player" 标签
        {
            isCollected = true;
            ScoreManager.instance.AddPoint(value);

            Destroy(gameObject); // 拾取后销毁物品
        }
    }
}

