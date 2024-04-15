using UnityEngine;

[CreateAssetMenu(menuName = "Create ItemData", fileName = "ItemData_")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _itemID;
    [SerializeField] private Sprite _itemSprite;

    public string ItemID => _itemID;
    public Sprite ItemSprite => _itemSprite;
}
