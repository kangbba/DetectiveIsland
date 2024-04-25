using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Character/CharacterData", order = 1)]
public class CharacterData : ScriptableObject{

    [SerializeField] private string _characterID;
    [SerializeField] private Character _characterPrefab;
    [SerializeField] private string _characterNameForUser;
    [SerializeField] private Color _characterColor;

    public string CharacterID { get => _characterID; }
    public Character CharacterPrefab { get => _characterPrefab; }
    public string CharacterNameForUser => _characterNameForUser;
    public Color CharacterColor => _characterColor;

    
}