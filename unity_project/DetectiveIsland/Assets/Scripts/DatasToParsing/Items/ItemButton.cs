using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemButton : DataButton
{
    [SerializeField] private Image _previewImg;
    [SerializeField] private Image _cursoredImg;
    private ItemData _itemData;

    public ItemData ItemData { get => _itemData; }

    public void Initialize(ItemData itemData, Action<string> onClick)
    {
        base.Initialize(itemData.ItemID, onClick);
        _itemData = itemData;
        if (_itemData.ItemSprite != null)
        {
            _previewImg.sprite = _itemData.ItemSprite;
        }
        SetSelectedImg(false);
    }

    public void SetSelectedImg(bool b){
        _cursoredImg.gameObject.SetActive(b);
    }
}
