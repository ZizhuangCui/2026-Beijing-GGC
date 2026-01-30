using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            collected = true;
            GameManager.Instance.OnDiamondCollected();
            gameObject.SetActive(false); 
        }
    }

    public void ResetDiamond()
    {
        collected = false;
        gameObject.SetActive(true);
    }
}
