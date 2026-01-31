using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TestTools;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class TaskFinishManager : Singleton<TaskFinishManager>
{
    private  readonly Dictionary<string, int> NumberMap = new Dictionary<string, int>
    {
        {"零", 0}, {"一", 1}, {"二", 2}, {"三", 3}, {"四", 4}, {"五", 5},
    };
    private List<Task> tasks = new List<Task>();
    public Task currentTask;
    public int currentTaskIndex = 0;
    public void TashParse(List<TaskData> taskDatas)
    {
        foreach(var taskData in taskDatas)
        {
            Task task = new Task();
            int n = 1;
            foreach (var word in taskData.selectsWords)
            {
                if (word.semantics != Semantics.Numeral) task.taskConditions.Add(word.text, false);
                else n = ConvertSingle(word.text);
            }
            task.requiredCount = n;

            tasks.Add(task);
        } 
        currentTask = tasks[currentTaskIndex];
    }
    public void SettaskConditions(string action,bool isCompleted)
    {
        currentTask.taskConditions[action] = isCompleted;
        currentTask.UpdateProgress();
        if (currentTask.isCompleted)
        {
            currentTaskIndex++;
            currentTask = tasks[currentTaskIndex];
        }
    }
    public  int ConvertSingle(string chineseChar)
    {
        if (NumberMap.TryGetValue(chineseChar, out int value))
        {
            return value;
        }
        return -1; 
    }
}
