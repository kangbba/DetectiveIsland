using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

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
    public static async UniTask DialogueTask(Dialogue dialogue){

        string characterID = dialogue.CharacterID;
        bool isRyan = characterID == "Ryan" || characterID == "Mono";
        CharacterData characterData = CharacterService.GetCharacterData(characterID);
        Character instancedCharacter = CharacterService.GetInstancedCharacter(characterID);
        if(instancedCharacter != null){
            CameraController.MoveX(instancedCharacter.transform.position.x / 10f, 1f);
        }
        for(int i = 0 ; i < dialogue.Lines.Count ; i++){
            Line line = dialogue.Lines[i];
            if(instancedCharacter != null){
                Debug.Log("로그");
                instancedCharacter.ChangeEmotion(line.EmotionID, .3f);
                instancedCharacter.StartTalking();
            }
            _dialoguePanel.SetCharacterText(characterData.CharacterNameForUser, characterData.CharacterColor);
            await _dialoguePanel.TypeLineTask(dialogue.Lines[i].Sentence, Color.white);
            if(instancedCharacter != null){
                Debug.Log("로그2");
                instancedCharacter.StopTalking();
            }
            await UniTask.WaitUntil(()=> Input.GetMouseButtonDown(0));
        }
    }
}
