using System;
using System.Collections.Generic;
using Aroka.ArokaUtils;
using UnityEngine;

public static class CharacterService
{
    private static CharacterPanel _characterPanel;
    private static List<Character> _characterPrefabs;
    
    public static event Action<string> OnCharacterTalk; // 말 걸리고있는 캐릭터 액션 모음

    public static void Load()
    {       
        _characterPanel = UIManager.Instance.CharacterPanel;
        _characterPrefabs = ArokaUtils.LoadResourcesFromFolder<Character>("CharacterPrefabs");
        Debug.Log("_characterPrefabs" + _characterPrefabs.Count);
    }
    public static Character GetCharacter(string characterID)
    {
        foreach (Character character in _characterPrefabs)
        {
            if (character.CharacterData.CharacterID == characterID)
            {
                return character;
            }
        }
        Debug.LogWarning("캐릭터를 찾을수 없음");
        return null;
    }
    public static CharacterData GetCharacterData(string characterID)
    {
        foreach (Character character in _characterPrefabs)
        {
            if (character.CharacterData.CharacterID == characterID)
            {
                return character.CharacterData;
            }
        }
        Debug.LogWarning("캐릭터 데이터를 찾을수 없음");
        return null;
    }
    public static void InitializeCharacters(List<PositionInit> positionInits, float totalTime){
        foreach(PositionInit positionInit in positionInits){
            MakeCharacter(positionInit.CharacterID, positionInit.PositionID, totalTime);
        }
    }
    public static void DestroyAllCharacters(float totalTime){
        _characterPanel.DestroyAllCharacters(totalTime);
    }
    public static void MakeCharacter(string characterID, string positionID, float totalTime){
        _characterPanel.MakeCharacter(characterID, positionID, totalTime);
    }
    public static void DestroyCharacter(string characterID, float totalTime){
        _characterPanel.DestroyCharacter(characterID, totalTime);
    }

    public static void NotifyTalking(string characterID)
    {
        Debug.Log($"현재 말을 걸고 있는 캐릭터 {characterID} 이벤트로 발행");
        OnCharacterTalk?.Invoke(characterID);
    }

    public static void PositionChange(string characterID, string positionID, float totalTime){
        _characterPanel.PositionChange(characterID, positionID, totalTime);
    }
}
