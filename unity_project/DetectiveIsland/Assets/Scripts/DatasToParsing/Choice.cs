using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Choice
{
    private string _title;
    private List<Element> _elements;

    public Choice(string title, List<Element> elements)
    {
        this._title = title;
        this._elements = elements;
    }
}
