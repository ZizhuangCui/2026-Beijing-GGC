using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskItem : MonoBehaviour
{
    public string formatString;
    public List<PositionWordBank> positionWordBanks; // 每个位置的词库

    [System.Serializable]
    public class PositionWordBank
    {
        public string positionKey; // 位置键名，如："名词1"
        public List<string> wordBank; // 该位置的词库列表
    }
    public TextMeshProUGUI taskText;
    private void Awake()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
