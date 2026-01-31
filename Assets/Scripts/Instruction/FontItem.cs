using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FontItem : MonoBehaviour
{
    public FontData fontData;
    public TextMeshProUGUI font;
    // Start is called before the first frame update
    void Start()
    {
        font.text = fontData.text;
        font.fontSize = 25;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerDown()
    {
        FontManager.Instance.AddFont(fontData.text);
    }
}
