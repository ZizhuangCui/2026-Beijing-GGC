using AI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using InstructionSystem;

public class TaskManager : MonoBehaviour
{
    class WordBanksCell
    {
        public Semantics semantics;
        public List<string> wordBanks = new List<string>();
    }
    public List<TaskData> taskDatas= new List<TaskData>();
    private Dictionary<string, WordBanksCell> globalWordBanks = new Dictionary<string, WordBanksCell>();
    public GameObject taskPrefabs;
    private string overrideUserPrompt =
            "帮我润色A这句话，要求生成一个简短、可执行的任务";
    void Awake()
    {
      InitializeTaskFormats();
      InitializeWordBanks();
      GeneratedTask();
      
    }
    
    private WordData SelectRandomWord(string placeholder)
    {
        WordBanksCell cell = globalWordBanks[placeholder];
        WordData wordData = new WordData();
        wordData.semantics = cell.semantics;
        wordData.text = cell.wordBanks[Random.Range(0, cell.wordBanks.Count)];
        return wordData;
    }
    void InitializeTaskFormats()
    {
        
        TaskData taskData = new TaskData();
        taskData.formatString = "拿 {A} 扔到 {B}";
        taskDatas.Add(taskData);

        taskData = new TaskData();
        taskData.formatString = "{C} {D}";
        taskDatas.Add(taskData);

        taskData = new TaskData();
        taskData.formatString = "到{E}上做出行为{F}{G}次";
        taskDatas.Add(taskData);
    }
    void InitializeWordBanks()
    {
        WordBanksCell cell = new WordBanksCell();
        cell.wordBanks = new List<string> { "网球", "羽毛球", "萝卜" };
        cell.semantics = Semantics.Noun;
        globalWordBanks.Add("A", cell);

        cell = new WordBanksCell();
        cell.wordBanks = new List<string> { "桌子", "红色旗子", "蓝色旗子", "草" };
        cell.semantics = Semantics.Noun;
        globalWordBanks.Add("B", cell);

        cell = new WordBanksCell();
        cell.wordBanks = new List<string> { "拿", "吃", "扔", "洒","叫"};
        cell.semantics = Semantics.Verb;
        globalWordBanks.Add("C", cell);

        cell = new WordBanksCell();
        cell.wordBanks = new List<string> { "奶茶", "可乐", "汉堡", "饭团","萝卜","纸巾" };
        cell.semantics = Semantics.Noun;
        globalWordBanks.Add("D", cell);

        cell = new WordBanksCell();
        cell.wordBanks = new List<string> { "红色旗子", "蓝色旗子", "黄色旗子"};
        cell.semantics = Semantics.Noun;
        globalWordBanks.Add("E", cell);

        cell = new WordBanksCell();
        cell.wordBanks = new List<string> { "叫","跳" };
        cell.semantics = Semantics.Verb;
        globalWordBanks.Add("F", cell);

        cell = new WordBanksCell();
        cell.wordBanks = new List<string> { "一", "二", "三"};
        cell.semantics = Semantics.Adjective;
        globalWordBanks.Add("G", cell);
    }
    void GeneratedTask()
    {
        for (int i = 0; i < taskDatas.Count; i++)
        {
            GameObject task = Instantiate(taskPrefabs);
            task.transform.parent = transform;
            TaskItem item = task.GetComponent<TaskItem>();
            var taskData = taskDatas[i];
            var placeholders = ExtractPlaceholders(taskData.formatString);
            Debug.Log(taskData.formatString);
            foreach (var placeholder in placeholders)
            {
                WordData selectedWord = SelectRandomWord(placeholder);

                if (!string.IsNullOrEmpty(selectedWord.text))
                {

                    taskData.formatString = taskData.formatString.Replace("{" + placeholder + "}", selectedWord.text);
                    taskData.selectsWords.Add(selectedWord);
                }
                else
                {
                    Debug.LogWarning($"未找到位置 {placeholder} 的可用词库");
                }
            }
            item.taskData = taskData;
            string  prompt = overrideUserPrompt.Replace("A", "\"" + taskData.formatString + "\"");
            Debug.Log(prompt);

            KimiTaskClient.
                     Instance.GenerateTask(
                     prompt,
                     onSuccess: (t) => item.taskText.text = t,
                     onError: (e) => item.taskText.text = "Fail：\n" + e
                 );
        }
    }
    private List<string> ExtractPlaceholders(string format)
    {
        List<string> placeholders = new List<string>();

        int startIndex = 0;
        while (startIndex < format.Length)
        {
            int openBrace = format.IndexOf('{', startIndex);
            if (openBrace == -1) break;

            int closeBrace = format.IndexOf('}', openBrace);
            if (closeBrace == -1) break;

            string placeholder = format.Substring(openBrace + 1, closeBrace - openBrace - 1);
            placeholders.Add(placeholder);

            startIndex = closeBrace + 1;
        }

        return placeholders;
    }
}
