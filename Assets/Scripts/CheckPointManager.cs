using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance;

    [SerializeField] private Transform startPoint;

    [SerializeField] private Transform currentRespawnPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentRespawnPoint = startPoint;
    }
    public void updateRespawnPoint(Transform newPoint)
    {
        currentRespawnPoint = newPoint;
    }

    public Transform getRespawnPoint()
    {
        return currentRespawnPoint;
    }
}
