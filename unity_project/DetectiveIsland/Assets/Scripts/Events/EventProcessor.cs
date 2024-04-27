using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public static class EventProcessor
{
    public static async UniTask ScenarioTask(Scenario scenario){

        Debug.Log("이벤트가 발동됨");
        //대화창 On
        DialogueService.ClearPanel();
        DialogueService.SetOnPanel(true, 1f);
        await UniTask.WaitForSeconds(1f);
        
        Debug.Log("이벤트 실행이 돌입");
        List<Element> elements = scenario.Elements;
        await   (ProcessElementsTask(elements));

        //대화창 Off
        DialogueService.SetOnPanel(false, 1f);
        await UniTask.WaitForSeconds(1f);

    }
    private static async UniTask ProcessElementsTask(List<Element> elements){

        foreach(Element element in elements){
            await ProcessElementTask(element);
        }
    }
    
    public static async UniTask ProcessElementTask(Element element){

        Debug.Log(element.GetType());
        if(element is PositionChange){
            PositionChange positionChange = element as PositionChange;
            CharacterService.InstantiateCharacter(positionChange.CharacterID, positionChange.PositionID);

            await UniTask.WaitForSeconds(1f);
        }
        else if(element is Dialogue){
            Dialogue dialogue = element as Dialogue;
            await DialogueService.DialogueTask(dialogue);
        }
        else if(element is ChoiceSet){

            ChoiceSet choiceSet = element as ChoiceSet;
            Choice selectedChoice = await ChoiceSetService.ChoiceSetTask(choiceSet);
            Debug.Log($"{selectedChoice.Title}을 골랐다!");
            await ProcessElementsTask(selectedChoice.Elements);

        }
        else if(element is ItemDemand){
            ItemDemand itemDemand = element as ItemDemand;
            foreach(Dialogue dialogue in itemDemand.Dialogues){
                await DialogueService.DialogueTask(dialogue);
            }
            await ItemUIService.ItemDemandTask(
                targetItemID : itemDemand.ItemID,
                successTask : ProcessElementsTask(itemDemand.SuccessElements),
                failTask : ProcessElementsTask(itemDemand.FailElements),
                cancelTask : ProcessElementsTask(itemDemand.FailElements)
            );
        }
        else if(element is AssetChange){
            AssetChange assetChange = element as AssetChange;
            await ItemService.AssetChangeTask(assetChange);
        }
    }
    
    public static void PositionInits(List<PositionChange> positionChanges){
        foreach(PositionChange positionChange in positionChanges){
            CharacterService.InstantiateCharacter(positionChange.CharacterID, positionChange.PositionID);
        }
    }
}
