using System;
using System.Collections.Generic;
using System.Linq;
using Aroka.ArokaUtils;
using UnityEngine;
using UnityEngine.TextCore.Text;

public static class CharacterService
{
    private static List<CharacterData> _characterDatas; 

    public static void Load()
    {       
        _characterDatas = ArokaUtils.LoadScriptableDatasFromFolder<CharacterData>("CharacterDatas");
    }

    // CharacterData 검색 함수
    public static CharacterData GetCharacterData(string characterID)
    {
        return _characterDatas.FirstOrDefault(data => data.CharacterID == characterID);
    }



}
