using UnityEngine;
using TMPro;
using System;

public class ChoiceButton : DataButton
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    private Choice _choice;

    public Choice Choice { get => _choice; }

    public void Initialize(Choice choice, Action<string> action)
    {
        _choice = choice;
        _buttonText.text = choice.Title;
        SetupButton(choice.Title, action);
    }
}
