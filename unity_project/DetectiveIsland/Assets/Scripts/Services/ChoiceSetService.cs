using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChoiceSetService
{
    private static ChoiceSetPanel _choiceSetPanel;
    public static void Initialize(){
        _choiceSetPanel = UIManager.Instance.ChoiceSetPanel; 
    }

    public static IEnumerator MakeChoiceBtnsAndWaitRoutine(ChoiceSet choiceSet){
        _choiceSetPanel.CreateChoiceBtns(choiceSet);
        _choiceSetPanel.OpenPanel(true, 1f);
        yield return new WaitForSeconds(1f);
        yield return _choiceSetPanel.AwaitChoiceBtnSelected();
        _choiceSetPanel.OpenPanel(false, 1f);
        yield return _choiceSetPanel.SelectedChoice;
    }

    public static void OpenPanel(bool b, float totalTime){
        _choiceSetPanel.OpenPanel(b, totalTime);
    }
}
