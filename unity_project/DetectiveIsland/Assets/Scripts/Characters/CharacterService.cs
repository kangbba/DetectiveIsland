using System;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using UnityEngine;
using UnityEngine.TextCore.Text;

public static class CharacterService
{
    private static CharacterPanel _characterPanel;
    private static List<CharacterData> _characterDatas; 
    private static List<Character> _curCharacters = new List<Character>();

    public static List<Character> CurCharacters { get => _curCharacters; set => _curCharacters = value; }

    public static void Load()
    {       
        _characterPanel = UIManager.Instance.CharacterPanel;
        _characterDatas = ArokaUtils.LoadScriptableDatasFromFolder<CharacterData>("CharacterDatas");
    }

    // CharacterData 검색 함수
    public static CharacterData GetCharacterData(string characterID)
    {
        return _characterDatas.FirstOrDefault(data => data.CharacterID == characterID);
    }
    public static Character GetInstancedCharacter(string characterID){

        return _curCharacters.FirstOrDefault(character => character.CharacterID == characterID);

    }

    public static void InstantiateCharacterThenFadeIn(string characterID, string positionID, string initialEmotionID, float totalTime)
    {
        CharacterData characterData = GetCharacterData(characterID);

        if (characterData == null)
        {
            Debug.LogWarning("characterPlan not found: " + characterID);
            return;
        }

        Character characterInstance = GameObject.Instantiate(characterData.CharacterPrefab, _characterPanel.transform);
        characterInstance.Initialize(characterData.CharacterID, positionID);
        characterInstance.SetEmotion(initialEmotionID, totalTime);
        _curCharacters.Add(characterInstance);
    }

    public static void FadeOutCharacterThenDestroy(string characterID, float totalTime)
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

}
