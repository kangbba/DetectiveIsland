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
    public static List<PositionChange> GetLastPosition(Scenario scenario)
    {
        Dictionary<string, PositionChange> positionChanges = new Dictionary<string, PositionChange>();

        foreach (Element element in scenario.Elements)
        {
            if (element is PositionChange positionChange)
            {
                positionChanges[positionChange.CharacterID] = positionChange; // 중복 시 최신 정보로 덮어씀
            }
        }

        return new List<PositionChange>(positionChanges.Values);
    }

    public static void InstantiateCharacter(string characterID, string positionID)
    {
        CharacterData characterData = GetCharacterData(characterID);

        if (characterData == null)
        {
            Debug.LogWarning("characterPlan not found: " + characterID);
            return;
        }

        Character characterInstance = GameObject.Instantiate(characterData.CharacterPrefab, _characterPanel.transform);
        characterInstance.Initialize(characterData.CharacterID);
        characterInstance.transform.localPosition = CalculatePosition(positionID);
        _curCharacters.Add(characterInstance);
    }

    public static void DestroyCharacter(string characterID)
    {
        Character character = GetInstancedCharacter(characterID);
        if (character != null)
        {
            GameObject.Destroy(character.gameObject);
            _curCharacters.Remove(character);
        }
        else
        {
            Debug.LogWarning("Character to destroy not found: " + characterID);
        }
    }
    public static void DestoryAllCharacters()
    {
        foreach (Character character in _curCharacters)
        {
            character.FadeOutAndDestroy(1f);
        }
        _curCharacters.Clear();
    }
    private static Vector3 CalculatePosition(string positionID)
    {
        Vector3 newPosition = Vector3.zero;
        switch (positionID)
        {
            case "Left":
                newPosition = new Vector3(-8f, 0f, 0f);
                break;
            case "Middle":
                newPosition = new Vector3(0f, 0f, 0f);
                break;
            case "Right":
                newPosition = new Vector3(8f, 0f, 0f);
                break;
        }
        return newPosition;
    }

}
