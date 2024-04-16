using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChoiceSet : Element
{
    private List<Dialogue> _dialogues;
    private List<Choice> _choices;

    public ChoiceSet(List<Dialogue> dialogues, List<Choice> choices)
    {
        this._dialogues = dialogues;
        this._choices = choices;
    }

    public List<Dialogue> Dialogues { get => _dialogues;}
    public List<Choice> Choices { get => _choices; }
}