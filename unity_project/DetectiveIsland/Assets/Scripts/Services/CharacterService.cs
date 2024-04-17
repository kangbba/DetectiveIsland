using System.Collections.Generic;
using Aroka.ArokaUtils;
using UnityEngine;

public static class CharacterService
{
    private static CharacterPanel _characterPanel;
    private static List<CharacterData> _characterDatas;
    
    public static void Initialize()
    {       
        _characterPanel = UIManager.Instance.CharacterPanel;
        _characterDatas = ArokaUtils.LoadDatasFromFolder<CharacterData>("CharacterDatas");
    }
    public static CharacterData GetCharacterData(string characterID)
    {
        foreach (CharacterData character in _characterDatas)
        {
            if (character.CharacterID == characterID)
            {
                return character;
            }
        }
        return null;
    }
    public static void SetOnPanel(bool b, float totalTime){
        _characterPanel.SetAnim(b, totalTime);
    }

}
