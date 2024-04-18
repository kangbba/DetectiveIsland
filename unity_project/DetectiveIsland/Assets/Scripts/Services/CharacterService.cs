using System.Collections.Generic;
using Aroka.ArokaUtils;
using UnityEngine;

public static class CharacterService
{
    private static CharacterPanel _characterPanel;
    private static List<Character> _characterPrefabs;
    
    public static void Initialize()
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
    public static void ShowCharacters(float totalTime){
        _characterPanel.ShowCharacters(totalTime);
    }
    public static void ClearCharacters(float totalTime){
        _characterPanel.ClearCharacters(totalTime);
    }

    public static void PositionChange(PositionChange positionChange){
         Debug.Log(positionChange.CharacterID + "/" + positionChange.PositionID);
        _characterPanel.RepositionCharacter(positionChange.CharacterID, positionChange.PositionID);
    }

}
