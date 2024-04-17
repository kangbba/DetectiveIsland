using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemButton : DataButton
{
    [SerializeField] private Image _previewImg;
    private ItemData _itemData;

    public void Initialize(ItemData itemData, Action<string> action)
    {
        _itemData = itemData;
        if (_itemData.ItemSprite != null)
        {
            _previewImg.sprite = _itemData.ItemSprite;
        }
        SetupButton(itemData.ItemID, action);
    }
}
