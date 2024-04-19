using System.Collections;
using System.Collections.Generic;
using Aroka.CoroutineUtils;
using UnityEngine;

public static class ChoiceSetService
{
    private static ChoiceSetPanel _choiceSetPanel;
    public static void Load(){
        _choiceSetPanel = UIManager.Instance.ChoiceSetPanel; 
    }

    public static IEnumerator ChoiceSetRoutine(ChoiceSet choiceSet){
        foreach(Dialogue dialogue in choiceSet.Dialogues){
            yield return CoroutineUtils.StartCoroutine(DialogueService.DialogueRoutine(dialogue));
        }
        _choiceSetPanel.CreateChoiceBtns(choiceSet);
        Choice selectedChoice = null;
        yield return CoroutineUtils.AwaitCoroutine<Choice>(_choiceSetPanel.GetSelectedChoiceRoutine(), result => {
            selectedChoice = result;
        });
        yield return selectedChoice;
    }

}
