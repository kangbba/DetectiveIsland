using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HospitalDoorPassword : Quest
{
    private string _correctPassword = "0713";
    private string _currentInput = "";

    [SerializeField] private Button[] _numberButtons;
    [SerializeField] private Button _enterButton;
    [SerializeField] private TextMeshProUGUI _displayText;

    public HospitalDoorPassword(QuestID questID) : base(questID)
    {
    }

    private void Awake()
    {
        // QuestID는 프리팹에서 설정됨
        foreach (var button in _numberButtons)
        {
            button.onClick.AddListener(() => OnNumberButtonClick(button.GetComponentInChildren<TextMeshProUGUI>().text));
        }
        _enterButton.onClick.AddListener(OnEnterButtonClick);
    }

    public override void StartQuest(Action onCompleteCallback)
    {
        base.StartQuest(onCompleteCallback);
        ResetInput();
        UpdateDisplay();
    }

    private void OnNumberButtonClick(string number)
    {
        if (_currentInput.Length < _correctPassword.Length)
        {
            _currentInput += number;
            UpdateDisplay();
        }
    }

    private void OnEnterButtonClick()
    {
        if (_currentInput == _correctPassword)
        {
            CompleteQuest();
        }
        else
        {
            ResetInput();
            UpdateDisplay();
        }
    }

    private void ResetInput()
    {
        _currentInput = "";
    }

    private void UpdateDisplay()
    {
        _displayText.text = _currentInput.PadRight(_correctPassword.Length, '_');
    }

    public override async UniTask WaitUntilComplete()
    {
        while (!IsCompleted)
        {
            await UniTask.Yield();
        }
    }
}
