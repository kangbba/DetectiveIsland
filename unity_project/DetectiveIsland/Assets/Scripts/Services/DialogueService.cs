using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueService
{
    private DialoguePanel _dialoguePanel;
    public void Initialize(){
        _dialoguePanel = UIManager.Instance.DialoguePanel;
    }
    public void SetOnPanel(bool b, float totalTime){
        _dialoguePanel.SetAnim(b, totalTime);
    }
}
