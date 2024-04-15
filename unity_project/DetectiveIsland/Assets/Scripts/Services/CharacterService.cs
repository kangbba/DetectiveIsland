using System.Collections.Generic;
using ArokaUtil;
using UnityEngine;

public class CharacterService
{
    private CharacterPanel _characterPanel;
    private List<CharacterData> _characterDatas;
    
    public void Initialize()
    {       
        _characterPanel = UIManager.Instance.CharacterPanel;
        _characterDatas = Utils.LoadDatasFromFolder<CharacterData>("CharacterDatas");
    }
    public CharacterData GetCharacterData(string characterID)
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
    public void SetOnPanel(bool b, float totalTime){
        _characterPanel.SetAnim(b, totalTime);
    }
}
