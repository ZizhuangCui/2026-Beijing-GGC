using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FontManager : Singleton<FontManager>
{
    public List<FontData> fontData;
    [HideInInspector]public List<FontData> selectFont;
    public GameObject fontPrefab;
    public TextMeshProUGUI instruction;
    [HideInInspector] public string instructionText;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (FontData data in fontData) 
        { 
            GameObject font =  Instantiate(fontPrefab);
            font.GetComponent<FontItem>().fontData = data;
            font.transform.parent = transform;  
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddFont(string text)
    {
        instructionText += text;
        instruction.text = instructionText; 
    }   
}
