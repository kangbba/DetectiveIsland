using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
public static class ChoiceSetService
{
    private static ChoiceSetPanel _choiceSetPanel;
    public static void Load(){
        _choiceSetPanel = UIManager.Instance.ChoiceSetPanel; 
    }

    public static async UniTask<Choice> ChoiceSetTask(ChoiceSet choiceSet){
        foreach(Dialogue dialogue in choiceSet.Dialogues){
            await DialogueService.DialogueTask(dialogue);
        }
        _choiceSetPanel.CreateChoiceBtns(choiceSet);
        Choice selectedChoice = await _choiceSetPanel.GetSelectedChoiceTask();
        return selectedChoice;
    }

}
