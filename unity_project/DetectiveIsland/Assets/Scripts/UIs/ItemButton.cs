using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemButton : DataButton
{
    [SerializeField] private Image _previewImg;
    [SerializeField] private Image _cursoredImg;
    private ItemData _itemData;

    public ItemData ItemData { get => _itemData; }

    public void Initialize(ItemData itemData, Action<string> onClick, Action<string> onEnterMouse, Action<string> onExitMouse)
    {
        base.Initialize(itemData.ItemID);
        base.ConnectOnClick(onClick);
        base.ConnectOnHover(onEnterMouse, onExitMouse);
        _itemData = itemData;
        if (_itemData.ItemSprite != null)
        {
            _previewImg.sprite = _itemData.ItemSprite;
        }
        SetCursored(false);
    }

    public void SetCursored(bool b){
        _cursoredImg.gameObject.SetActive(b);
    }
}
