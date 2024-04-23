using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCheckPanel : ItemPanel
{
    [SerializeField] private Button _enterBtn;
    [SerializeField] private Button _exitBtn;

    protected override void Start()
    {
        base.Start();

        _enterBtn.onClick.RemoveAllListeners();
        _enterBtn.onClick.AddListener(OnClickedExitBtn);

        _exitBtn.onClick.RemoveAllListeners();
        _exitBtn.onClick.AddListener(OnClickedExitBtn);
    }

    private void OnClickedEnterBtn()
    {
        ShowPanelOn(true, .3f);
    }
    private void OnClickedExitBtn()
    {
        ShowPanelOn(false, .3f);
    }

    public void ShowPanelOn(bool isOn, float totalTime){
        base.ShowPanel(isOn, totalTime);
    }
}
