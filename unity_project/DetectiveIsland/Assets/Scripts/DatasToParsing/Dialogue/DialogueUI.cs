using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public static class DialogueUI
{
    private static DialoguePanel _dialoguePanel;

    public static DialoguePanel DialoguePanel { get => _dialoguePanel; }

    public static void Load(){
        _dialoguePanel = UIManager.Instance.DialoguePanel;
        _dialoguePanel.Initialize();
    }
    


}
