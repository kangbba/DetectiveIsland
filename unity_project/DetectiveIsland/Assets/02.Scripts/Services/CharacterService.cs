using System;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum ECharacterID{
    Mono = 0,
    Ryan = 1,
    Steve = 2,
    Lisa = 3,
    Kate = 4,
    Tom = 5,
    Brian = 6,
    Joseph = 7,
    Rachel = 8,

}
public enum EChacterEmotion{
    Smile = 0,
    Sad = 1,
    Angry = 2,
    Noraml = 3,
}

public static class CharacterService
{
    private static Transform _characterPanel;
    private static List<CharacterData> _characterDatas; 
    private static List<Character> _curCharacters = new List<Character>();
    public static List<Character> CurCharacters { get => _curCharacters;  }

    public static void Load()
    {       
        _characterPanel = new GameObject("Character Panel").transform;
        _characterDatas = ArokaUtils.LoadScriptableDatasFromFolder<CharacterData>("CharacterDatas");
    }


    public static Character MakeCharacter(ECharacterID characterID, EChacterEmotion initialEmotionID, Vector3 targetPos, float totalTime){

        CharacterData characterData = GetCharacterData(characterID);
        if (characterData == null)
        {
            Debug.LogWarning("characterData not found: " + characterID);
            return null;
        }

        Character characterInstance = GameObject.Instantiate(characterData.CharacterPrefab, _characterPanel.transform);
        characterInstance.Initialize(characterData.CharacterID, targetPos);
        characterInstance.SetEmotion(initialEmotionID, totalTime);
        _curCharacters.Add(characterInstance);

        return characterInstance;
    }
    public static CharacterData GetCharacterData(ECharacterID characterID)
    {
        return _characterDatas.FirstOrDefault(data => data.CharacterID == characterID);
    }


    public static Vector3 GetLocalPosByPositionID(ECharacterPositionID positionID)
    {
        Vector3 newPosition = Vector3.zero;
        switch (positionID)
        {
            case ECharacterPositionID.Left:
                newPosition = new Vector3(-8f, 0f, 0f);
                break;
            case ECharacterPositionID.Middle:
                newPosition = new Vector3(0f, 0f, 0f);
                break;
            case ECharacterPositionID.Right:
                newPosition = new Vector3(8f, 0f, 0f);
                break;
        }
        return newPosition;
    }
    public static Character GetInstancedCharacter(ECharacterID characterID){

        return _curCharacters.FirstOrDefault(character => character.CharacterID == characterID);

    }
    public static void FadeOutCharacterThenDestroy(ECharacterID characterID, float totalTime)
    {
        Character character = GetInstancedCharacter(characterID);
        if (character != null)
        {
            character.FadeOutAndDestroy(totalTime);
            _curCharacters.Remove(character);
        }
        else
        {
            Debug.LogWarning("Character to destroy not found: " + characterID);
        }
    }
    public static void AllCharacterFadeOutAndDestroy(float totalTime)
    {
        foreach (Character character in _curCharacters)
        {
            character.FadeOutAndDestroy(totalTime);
        }
        _curCharacters.Clear();
    }
    public static void AllCharacterFadeIn(float totalTime){

        foreach (Character character in _curCharacters)
        {
            character.FadeInCurrentEmotion(totalTime);
        }
    }
    public static void AllCharacterFadeOut(float totalTime){
        
        foreach (Character character in _curCharacters)
        {
            character.FadeOutCurrentEmotion(totalTime);
        }
    }

    public static void StartCharacterTalking(ECharacterID characterID){
        Character character = GetInstancedCharacter(characterID);
        if(character == null){
            Debug.Log("해당 캐릭터가 없습니다");
            return;
        }
        character.StartTalking();
    }

    public static void StopCharacterTalking(ECharacterID characterID){
        Character character = GetInstancedCharacter(characterID);
        if(character == null){
            Debug.Log("해당 캐릭터가 없습니다");
            return;
        }
        character.StopTalking();
    }

    public static void SetCharacterEmotion(ECharacterID characterID, EChacterEmotion emotionID, float totalTime){
        Character character = GetInstancedCharacter(characterID);
        if(character == null){
            Debug.Log("해당 캐릭터가 없습니다");
            return;
        }
        character.SetEmotion(emotionID, totalTime);
    }


}
