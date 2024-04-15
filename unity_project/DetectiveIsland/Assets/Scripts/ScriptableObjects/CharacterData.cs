using UnityEngine;

[CreateAssetMenu(menuName = "Create CharacterData", fileName = "CharacterData_")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private string _characterID;
    [SerializeField] private string _characterNameForUser;
    [SerializeField] private Color _characterColor;

    public string CharacterID => _characterID;
    public string CharacterNameForUser => _characterNameForUser;
    public Color CharacterColor => _characterColor;
}
