using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using InstructionSystem;

public class WordItem : MonoBehaviour
{
    public WordData wordData;
    public TextMeshProUGUI word;
    // Start is called before the first frame update
    void Start()
    {
        word.text = wordData.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerDown()
    {
        WordManager.Instance.AddFont(wordData.text);
    }
}
