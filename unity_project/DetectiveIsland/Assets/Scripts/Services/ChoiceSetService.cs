using System.Collections;
using System.Collections.Generic;
using Aroka.CoroutineUtils;
using UnityEngine;

public static class ChoiceSetService
{
    private static ChoiceSetPanel _choiceSetPanel;
    public static void Initialize(){
        _choiceSetPanel = UIManager.Instance.ChoiceSetPanel; 
    }

    public static IEnumerator ChoiceSetRoutine(ChoiceSet choiceSet){
        foreach(Dialogue dialogue in choiceSet.Dialogues){
            yield return ArokaCoroutineUtils.StartCoroutine(DialogueService.DialogueRoutine(dialogue));
        }
        _choiceSetPanel.CreateChoiceBtns(choiceSet);
        Choice selectedChoice = null;
        yield return ArokaCoroutineUtils.AwaitCoroutine<Choice>(_choiceSetPanel.GetSelectedChoiceRoutine(), result => {
            selectedChoice = result;
        });
        yield return selectedChoice;
    }

}
