using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.Anim;
using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ChoiceSetPanel : ArokaAnimParent
{
    [SerializeField] private Transform _choiceBtnsParent;  // 버튼들을 담을 부모 Transform
    [SerializeField] private ChoiceButton _choiceBtnPrefab;  // 버튼 프리팹

    private List<ChoiceButton> _curChoiceBtns = new List<ChoiceButton>();  // 생성된 버튼들의 리스트
    private ChoiceButton _selectedChoiceBtn;


    public async UniTask<Choice> MakeChoiceBtnsAndWait(ChoiceSet choiceSet)
    {
        CreateChoiceBtns(choiceSet);
        OpenPanel();
        await UniTask.WaitForSeconds(.1f);
        _selectedChoiceBtn = null;
        while (_selectedChoiceBtn == null)
        {
            await UniTask.Yield();
        }
        float fadeOutTime = .2f;
        float selectedBtnDelayTime = .5f;
        foreach(ChoiceButton choiceButton in _curChoiceBtns){
            choiceButton.SetInteractable(false);
            bool isIdentical = _selectedChoiceBtn.Choice.Content == choiceButton.Choice.Content;
            Debug.Log(isIdentical);
            if(isIdentical){
                choiceButton.transform.EaseLocalScale(Vector3.zero, totalTime: fadeOutTime, delayTime: selectedBtnDelayTime);
            }
            else{
                choiceButton.transform.EaseLocalScale(Vector3.zero, totalTime: fadeOutTime);
            }
        }
        await UniTask.WaitForSeconds(fadeOutTime + selectedBtnDelayTime);
        ClosePanel();
        await UniTask.WaitForSeconds(1f);
        DestroyChoiceBtns();

        return _selectedChoiceBtn.Choice;
    }

    private void CreateChoiceBtns(ChoiceSet choiceSet)
    {
        // 시작 위치 계산
        float startY = (choiceSet.Choices.Count - 1) * 100;

        // 새 버튼 생성
        for (int i = 0; i < choiceSet.Choices.Count; i++)
        {
            Choice choice = choiceSet.Choices[i];
            ChoiceButton choiceButton = Instantiate(_choiceBtnPrefab, _choiceBtnsParent);
            
            // Initialize에 람다 표현식을 사용하여 Choice 객체를 직접 전달
            choiceButton.Initialize(choice, SelectChoice);

            // 버튼 위치 설정
            RectTransform rectTransform = choiceButton.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, startY - i * 200);

            _curChoiceBtns.Add(choiceButton);
        }
    }
    private void DestroyChoiceBtns(){
        // 기존 버튼 제거
        foreach (ChoiceButton btn in _curChoiceBtns)
        {
            Destroy(btn.gameObject);
        }
        _curChoiceBtns.Clear();
    }
    private void SelectChoice(string choiceTitle)
    {
        // _curChoiceBtns 리스트에서 choiceID와 일치하는 첫 번째 Choice 객체를 찾음
        var selectedButton = GetChoiceButton(choiceTitle);
        if (selectedButton != null)
        {
            _selectedChoiceBtn = selectedButton;
        }
    }

    private ChoiceButton GetChoiceButton(string choiceTitle){
        return _curChoiceBtns.FirstOrDefault(btn => btn.Choice.Content == choiceTitle);
    }

    public void OpenPanel()
    {
        base.SetOnAllChildren(true, .1f);
    }
    public void ClosePanel()
    {
        base.SetOnAllChildren(false, .1f);
    }
}
