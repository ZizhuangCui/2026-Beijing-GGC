using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Semantics
{
    Item,
    Location,
    Verb,
    Adjective,
    Numeral
}
[Serializable]
public class WordData
{
    public Semantics semantics;
    public string text;
    public string garbledText;
}
