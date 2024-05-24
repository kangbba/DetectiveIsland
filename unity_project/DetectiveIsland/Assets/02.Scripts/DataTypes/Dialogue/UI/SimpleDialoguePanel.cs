using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class SimpleDialoguePanel : MonoBehaviour
{
    [SerializeField] private ArokaAnimParent _arokaAnimParent;
    [SerializeField] private TextMeshProUGUI _textMeshPro;

    public async UniTask ShowSimpleDialogue(string[] sentences)
    {
        if (sentences == null || sentences.Length == 0)
        {
            Debug.LogError("Dialogue sentences are empty or null");
            return;
        }

        ResetPanel();  // 다이얼로그를 실행할 때마다 패널 초기화

        _arokaAnimParent.SetOnAllChildren(true, .5f);

        foreach (var sentence in sentences)
        {
            ShowLine(sentence);
            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        _arokaAnimParent.SetOnAllChildren(false, .5f);
        await UniTask.WaitForSeconds(.5f);
    }


    private void ShowLine(string sentence)
    {
        _textMeshPro.text = sentence;
    }

    private void ResetPanel()
    {
        _textMeshPro.text = string.Empty;
        _arokaAnimParent.SetOnAllChildren(false, .5f);
    }
}
