using UnityEngine;

[CreateAssetMenu(menuName = "Create ItemData", fileName = "ItemData_")]
public class ItemData : ScriptableObject
{
    [SerializeField] private EItemID _itemID;
    [SerializeField] private Sprite _itemSprite;
    [SerializeField] private string _itemNameForUser;
    [SerializeField] private string _itemDescription;

    public EItemID ItemID => _itemID;
    public Sprite ItemSprite => _itemSprite;

    public string ItemNameForUser { get => _itemNameForUser; }
    public string ItemDescription { get => _itemDescription; }
}
