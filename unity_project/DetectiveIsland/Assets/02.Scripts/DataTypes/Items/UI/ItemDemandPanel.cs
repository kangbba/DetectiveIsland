using System.Collections;
using System.Collections.Generic;
using Aroka.Anim;
using Aroka.ArokaUtils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ItemDemandPanel : ItemPanel
{
    [SerializeField] protected ArokaAnimParent _arokaAnimParent;
    [SerializeField] private Button _submitBtn;
    [SerializeField] private Button _exitBtn;
    private ItemButton _confirmedItemButton; // 사용자가 선택한 아이템 버튼

    private bool _isDemandCancel = false;

    protected void Start()
    {
        _submitBtn.onClick.AddListener(OnClickedSubmitBtn);
        _exitBtn.onClick.AddListener(OnClickedExitBtn);
    }
    public async UniTask<ItemData> OpenItemDemandPanelAndWait()
    {
        OpenPanel();
        _isDemandCancel = false;
        _confirmedItemButton = null; // 초기화

        while (_confirmedItemButton == null && !_isDemandCancel) // 버튼이 클릭될 때까지 기다림
        {
            await UniTask.Yield(); // 다음 프레임까지 대기
        }

        ClosePanel();
        await UniTask.WaitForSeconds(.3f);

        if (_confirmedItemButton != null)
        {
            //무언가 선택하고 제출버튼을 누른 경우
            return _confirmedItemButton.ItemData; // 선택된 아이템 데이터 반환
        }
        else
        {
            //취소로 나온 경우
            return null;
        }
    }

    public void OnClickedExitBtn(){
        ClosePanel();
        _isDemandCancel = true;
    }
    private void OnClickedSubmitBtn()
    {
        _confirmedItemButton = _selectedItemBtn; 
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

    private void Update(){
        if(_isOpen){
            _submitBtn.GetComponent<ArokaAnim>().SetAnim(_selectedItemBtn != null, .3f);
        }
    }
}
