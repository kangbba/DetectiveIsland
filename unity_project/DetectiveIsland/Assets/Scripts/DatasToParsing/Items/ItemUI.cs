using System;
using System.Collections;
using System.Collections.Generic;
using Aroka.Anim;
using Aroka.EaseUtils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class ItemUI
{
    private static ItemCheckPanel _itemCheckPanel;
    private static ItemDemandPanel _itemDemandPanel;
    private static ItemOwnPanel _itemOwnPanel;
    private static Button _itemCheckPanelEnterBtn;

    public static ItemCheckPanel ItemCheckPanel { get => _itemCheckPanel; }
    public static ItemDemandPanel ItemDemandPanel { get => _itemDemandPanel; }
    public static ItemOwnPanel ItemOwnPanel { get => _itemOwnPanel; }

    public static void Load(){

        _itemCheckPanel = UIManager.Instance.ItemCheckPanel;
        _itemDemandPanel = UIManager.Instance.ItemDemandPanel;
        _itemOwnPanel = UIManager.Instance.ItemOwnPanel;
        _itemCheckPanelEnterBtn = UIManager.Instance.ItemCheckPanelEnterBtn;

        _itemCheckPanelEnterBtn.onClick.RemoveAllListeners();
        _itemCheckPanelEnterBtn.onClick.AddListener(_itemCheckPanel.OpenPanel);
    }

    // ItemCheckPanel을 통해 ItemCheckPanelButton을 표시하는 메소드
    public static void ShowItemCheckPanelEnterButton()
    {   
        _itemCheckPanelEnterBtn.GetComponent<ArokaAnim>().SetAnim(true, .3f);
    }

    public static void HideItemCheckPanelEnterButton()
    {   
        _itemCheckPanelEnterBtn.GetComponent<ArokaAnim>().SetAnim(false, .3f);
    }


}
