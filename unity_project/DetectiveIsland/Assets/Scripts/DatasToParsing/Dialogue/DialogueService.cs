using System.Collections;
using System.Collections.Generic;
using Aroka.CoroutineUtils;
using UnityEngine;

public static class DialogueService
{
    private static DialoguePanel _dialoguePanel;
    public static void Load(){
        _dialoguePanel = UIManager.Instance.DialoguePanel;
        _dialoguePanel.Initialize();
    }
    public static void ClearPanel(){
        _dialoguePanel.ClearPanel();
    }
    public static void SetOnPanel(bool b, float totalTime){
        _dialoguePanel.SetAnim(b, totalTime);
    }
    public static IEnumerator DialogueRoutine(Dialogue dialogue){

        string characterID = dialogue.CharacterID;
        CharacterData characterData = CharacterService.GetCharacterData(characterID);
        Character instancedCharacter = CharacterService.GetInstancedCharacter(characterID);
        bool isRyan = (characterID == "Ryan" || characterID == "Mono");
        for(int i = 0 ; i < dialogue.Lines.Count ; i++){
            Line line = dialogue.Lines[i];
            if(instancedCharacter != null){
                instancedCharacter.ChangeEmotion(line.EmotionID, 1f);
            }
            _dialoguePanel.SetCharacterText(characterData.CharacterNameForUser, characterData.CharacterColor);
            yield return CoroutineUtils.StartCoroutine(_dialoguePanel.TypeLineRoutine(dialogue.Lines[i].Sentence, Color.white));
            yield return CoroutineUtils.WaitUntil(()=> Input.GetMouseButtonDown(0));
        }
    }
}
