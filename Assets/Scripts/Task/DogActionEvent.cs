using System.Collections.Generic;
using UnityEngine;

public class PlayerActionManager : Singleton<PlayerActionManager>
{
    List<string> actions = new List<string>();
    public void AddAction(string action)
    {
        actions.Add(action);
    }
    public void RemoveAction(string action)
    {
        actions.Remove(action);
    }
    public void TaskTesting()
    {
                                     
    }
}