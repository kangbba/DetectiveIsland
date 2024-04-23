using System.Collections.Generic;
using Aroka.Anim;
using UnityEngine;
using UnityEngine.UI;

public class ItemCheckPanel : ItemPanel
{

    [SerializeField] private Button _exitBtn;
    
    protected override void Start()
    {
        base.Start();
        _exitBtn.onClick.AddListener(OnClickedExitBtn);
    }

    public override void OpenPanel(bool isOn, float totalTime){
        base.OpenPanel(isOn, totalTime);
    }

    public void OnClickedExitBtn(){
        base.OpenPanel(false, .3f);
    }

}
