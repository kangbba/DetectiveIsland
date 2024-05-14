using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Choice
{
    private string _content;
    private List<Element> _elements;

    public Choice(string content, List<Element> elements)
    {
        this._content = content;
        this._elements = elements;
    }

    public string Content { get => _content; set => _content = value; }
    public List<Element> Elements { get => _elements; set => _elements = value; }

}
