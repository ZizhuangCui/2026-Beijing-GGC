using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WordManager : Singleton<WordManager>
{
    public List<WordData> wordDatas;
    [HideInInspector]public List<FontData> selectFont;
    public GameObject fontPrefab;
    public TextMeshProUGUI instruction;
    [HideInInspector] public string instructionText;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (WordData data in wordDatas) 
        { 
            GameObject font =  Instantiate(fontPrefab);
            font.GetComponent<WordItem>().wordData = data;
            font.transform.parent = transform;  
        }

    }
   
    void Update()
    {
        
    }
    public void AddFont(string text)
    {
        instructionText += text;
        instruction.text = instructionText; 
    }   
}
