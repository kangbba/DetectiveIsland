using System.Collections;
using System.Collections.Generic;
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
    public static IEnumerator DisplayTextRoutine(Dialogue dialogue){

        for(int i = 0 ; i < dialogue.Lines.Count ; i++){
            CharacterData characterPlan = CharacterService.GetCharacterData(dialogue.CharacterID);
            _dialoguePanel.SetCharacterText(dialogue.CharacterID, characterPlan.CharacterColor);
            yield return ArokaCoroutineUtils.StartCoroutine(_dialoguePanel.TypeLineRoutine(dialogue.Lines[i].Sentence, Color.white));
            yield return ArokaCoroutineUtils.WaitUntil(()=> Input.GetMouseButtonDown(0));
        }
    }
}
