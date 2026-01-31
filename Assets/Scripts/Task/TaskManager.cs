using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    class WordBanksCell
    {
        public Semantics semantics;
        public List<string> wordBanks = new List<string>();
    }
    public List<TaskData> taskDatas= new List<TaskData>();
    private Dictionary<string, WordBanksCell> globalWordBanks = new Dictionary<string, WordBanksCell>();
    // Start is called before the first frame update
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
        taskData.formatString = "ÄÃ {A} ÈÓµ½ {B}";
        taskDatas.Add(taskData);                                           
    }
    void InitializeWordBanks()
    {
        WordBanksCell cell = new WordBanksCell();
        cell.wordBanks = new List<string> { "ÍøÇò", "ÓðÃ«Çò", "ÂÜ²·" };
        cell.semantics = Semantics.Noun;
        globalWordBanks.Add("A", cell);
        cell = new WordBanksCell();
        cell.wordBanks = new List<string> { "×À×Ó", "ºìÉ«Æì×Ó", "À¶É«Æì×Ó", "²Ý" };
        cell.semantics = Semantics.Noun;
        globalWordBanks.Add("B", cell);
    }
    void GeneratedTask()
    {
        for (int i = 0; i < taskDatas.Count; i++)
        {
            var taskData = taskDatas[i];
            var placeholders = ExtractPlaceholders(taskData.formatString);
            foreach (var placeholder in placeholders)
            {
                WordData selectedWord = SelectRandomWord(placeholder);

                if (!string.IsNullOrEmpty(selectedWord.text))
                {
                    // Ìæ»»Õ¼Î»·û
                    taskData.formatString = taskData.formatString.Replace("{" + placeholder + "}", selectedWord.text);
                    taskData.selectsWords.Add(selectedWord);
                }
                else
                {
                    Debug.LogWarning($"Î´ÕÒµ½Î»ÖÃ {placeholder} µÄ¿ÉÓÃ´Ê¿â");
                }
            }
            Debug.Log(taskData.formatString);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
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
