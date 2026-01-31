using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;


public class Task
{
    public bool isCompleted = false;

    public Dictionary<string,bool> taskConditions = new Dictionary<string, bool>();
    public int requiredCount;
    public int currentCount = 0;
    public void UpdateProgress()
    {
        if(taskConditions.All(kvp => kvp.Value))
        {
            currentCount++;
            if (currentCount >= requiredCount)
                isCompleted = true;
        }
    }
}
