using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    bool isActive = false;
    [SerializeField] private float delayTime = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActive)
        {
            isActive = true;
            CheckPointManager.Instance.updateRespawnPoint(transform);
            StartCoroutine(LoadNextSceneWithDelay(delayTime)); 
        }
    }

    private IEnumerator LoadNextSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        SceneManager.LoadScene(2); 
    }
}
