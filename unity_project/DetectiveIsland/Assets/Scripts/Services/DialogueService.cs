using System.Collections;
using System.Collections.Generic;
using Aroka.CoroutineUtils;
using UnityEngine;

public static class DialogueService
{
    private static DialoguePanel _dialoguePanel;
    public static void Initialize(){
        _dialoguePanel = UIManager.Instance.DialoguePanel;
        _dialoguePanel.Initialize();
    }
    public static void SetOnPanel(bool b, float totalTime){
        _dialoguePanel.SetAnim(b, totalTime);
    }
    public static IEnumerator DialogueRoutine(Dialogue dialogue){

        for(int i = 0 ; i < dialogue.Lines.Count ; i++){
            CharacterData characterData = CharacterService.GetCharacterData(dialogue.CharacterID);
            _dialoguePanel.SetCharacterText(dialogue.CharacterID, characterData.CharacterColor);
            yield return ArokaCoroutineUtils.StartCoroutine(_dialoguePanel.TypeLineRoutine(dialogue.Lines[i].Sentence, Color.white));
            yield return ArokaCoroutineUtils.WaitUntil(()=> Input.GetMouseButtonDown(0));
        }
    }
}
