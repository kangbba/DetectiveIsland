using System.Collections.Generic;
using Aroka.Anim;
using UnityEngine;
using UnityEngine.UI;

public class ItemCheckPanel : ItemPanel
{

    [SerializeField] protected ArokaAnimParent _arokaAnimParent;
    [SerializeField] private Button _exitBtn;
    
    private void Start()
    {
        _exitBtn.onClick.AddListener(OnClickedExitBtn);
        ClosePanel();
    }
    public void OpenPanel()
    {
        List<ItemData> itemDatas = null;
        base.Initialize(itemDatas);
        _arokaAnimParent.SetOnAllChildren(true, .3f);
    }
    public void ClosePanel()
    {
        _arokaAnimParent.SetOnAllChildren(false, .3f);
    }

    public void OnClickedExitBtn(){
        ClosePanel();
    }

}
