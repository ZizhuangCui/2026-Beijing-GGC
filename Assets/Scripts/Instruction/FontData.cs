using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Semantics
{
    Noun,
    Verb,
    Adjective
}
[Serializable]
public class FontData
{
    public Semantics Semantics;
    public string text;
    public string garbledText;
}
