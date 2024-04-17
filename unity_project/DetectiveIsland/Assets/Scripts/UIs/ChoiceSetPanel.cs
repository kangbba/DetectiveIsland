using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aroka.Anim;
using Aroka.ArokaUtils;
using UnityEngine;

public class ChoiceSetPanel : MonoBehaviour
{
    [SerializeField] private Transform _choiceBtnsParent;  // 버튼들을 담을 부모 Transform
    [SerializeField] private ChoiceButton _choiceBtnPrefab;  // 버튼 프리팹

    private List<ChoiceButton> _curChoiceBtns = new List<ChoiceButton>();  // 생성된 버튼들의 리스트
    private Choice _selectedChoice;  // 선택된 선택지

    private ArokaAnim[] ChildrenAnims => transform.GetComponentsInChildren<ArokaAnim>();

    public Choice SelectedChoice { get => _selectedChoice;  }

    public IEnumerator AwaitChoiceBtnSelected()
    {
        _selectedChoice = null;
        while (_selectedChoice == null)
        {
            yield return null;
        }
        Debug.Log($"Selected choice: {_selectedChoice}");
    }
    
    public void CreateChoiceBtns(ChoiceSet choiceSet)
    {
        // 기존 버튼 제거
        foreach (ChoiceButton btn in _curChoiceBtns)
        {
            Destroy(btn.gameObject);
        }
        _curChoiceBtns.Clear();

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
    private void SelectChoice(string choiceTitle)
    {
        // _curChoiceBtns 리스트에서 choiceID와 일치하는 첫 번째 Choice 객체를 찾음
        var selectedButton = _curChoiceBtns.FirstOrDefault(btn => btn.Choice.Title == choiceTitle);
        if (selectedButton != null)
        {
            _selectedChoice = selectedButton.Choice;
            Debug.Log($"Selected choice: {_selectedChoice.Title}");
        }
    }

    public void OpenPanel(bool b, float totalTime){
        ChildrenAnims.SetAnims(b, totalTime);
    }



}
