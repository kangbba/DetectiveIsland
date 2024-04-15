using UnityEngine;

public class CharacterManager : Manager<CharacterData>
{
    public override void Initialize(string folderName, GameObject mainPanel)
    {
        base.Initialize(folderName, mainPanel);
        Debug.Log("PlaceManager initialized with place panel: " + MainPanel.name);
    }
    public CharacterData GetCharacterData(string characterID)
    {
        foreach (CharacterData character in _dataList)
        {
            if (character.CharacterID == characterID)
            {
                return character;
            }
        }
        return null;
    }
}
