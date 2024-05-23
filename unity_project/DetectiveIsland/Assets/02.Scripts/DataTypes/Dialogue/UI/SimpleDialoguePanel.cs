using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class SimpleDialoguePanel : MonoBehaviour
{
    [SerializeField] private ArokaAnimParent _arokaAnimParent;
    [SerializeField] private TextMeshProUGUI _textMeshPro;

    private string[] _dialogueLines;
    private int _currentLineIndex;
    private bool _isPanelActive;

    private void Awake()
    {
        _isPanelActive = false;
    }

    public async UniTask ShowDialogue(string[] dialogueLines)
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogError("Dialogue lines are empty or null");
            return;
        }

        ResetPanel();  // 다이얼로그를 실행할 때마다 패널 초기화

        _dialogueLines = dialogueLines;
        _currentLineIndex = 0;
        _isPanelActive = true;
        _arokaAnimParent.SetOnAllChildren(true, .5f);

        for (_currentLineIndex = 0; _currentLineIndex < _dialogueLines.Length; _currentLineIndex++)
        {
            ShowCurrentLine();
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        ClosePanel();
    }

    private void ShowCurrentLine()
    {
        if (_currentLineIndex < _dialogueLines.Length)
        {
            _textMeshPro.text = _dialogueLines[_currentLineIndex];
        }
    }

    private void ClosePanel()
    {
        _arokaAnimParent.SetOnAllChildren(false, .5f);
        _isPanelActive = false;
    }

    private void ResetPanel()
    {
        _textMeshPro.text = string.Empty;
        _dialogueLines = null;
        _currentLineIndex = 0;
        _isPanelActive = false;
        _arokaAnimParent.SetOnAllChildren(false, .5f);
    }
}
