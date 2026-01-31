using System;



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
