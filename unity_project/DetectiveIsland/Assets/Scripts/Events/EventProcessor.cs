using System.Collections;
using System.Collections.Generic;
using Aroka.CoroutineUtils;
using Aroka.JsonUtils;
using UnityEngine;

public static class EventProcessor
{
    public static IEnumerator ScenarioRoutine(Scenario scenario){

        Debug.Log("이벤트가 발동됨");
        //대화창 On
        DialogueService.ClearPanel();
        DialogueService.SetOnPanel(true, 1f);
        yield return new WaitForSeconds(1f);
        
        Debug.Log("이벤트 실행이 돌입");
        List<Element> elements = scenario.Elements;
        yield return CoroutineUtils.StartCoroutine(ProcessElementsRoutine(elements));

        //대화창 Off
        DialogueService.SetOnPanel(false, 1f);
        yield return new WaitForSeconds(1f);

    }
    private static IEnumerator ProcessElementsRoutine(List<Element> elements){

        foreach(Element element in elements){
            yield return ProcessElementRoutine(element);
        }
    }
    
    public static IEnumerator ProcessElementRoutine(Element element){

        Debug.Log(element.GetType());
        if(element is PositionChange){
            PositionChange positionChange = element as PositionChange;
            CharacterService.InstantiateCharacter(positionChange.CharacterID, positionChange.PositionID);

            yield return new WaitForSeconds(1f);
        }
        else if(element is Dialogue){
            Dialogue dialogue = element as Dialogue;
            yield return CoroutineUtils.StartCoroutine(DialogueService.DialogueRoutine(dialogue));
        }
        else if(element is ChoiceSet){

            ChoiceSet choiceSet = element as ChoiceSet;
            Choice selectedChoice = null;
            yield return CoroutineUtils.AwaitCoroutine<Choice>(ChoiceSetService.ChoiceSetRoutine(choiceSet), result => {
                selectedChoice = result;
            });
            Debug.Log($"{selectedChoice.Title}을 골랐다!");
            yield return CoroutineUtils.StartCoroutine(ProcessElementsRoutine(selectedChoice.Elements));

        }
        else if(element is ItemDemand){
            ItemDemand itemDemand = element as ItemDemand;
            foreach(Dialogue dialogue in itemDemand.Dialogues){
                yield return CoroutineUtils.StartCoroutine(DialogueService.DialogueRoutine(dialogue));
            }
            yield return ItemUIService.ItemDemandRoutine(
                targetItemID : itemDemand.ItemID,
                successAction : () => ProcessElementsRoutine(itemDemand.SuccessElements),
                failAction : () => ProcessElementsRoutine(itemDemand.FailElements),
                cancelAction : () => ProcessElementsRoutine(itemDemand.FailElements)
            );
        }
        else if(element is AssetChange){
            AssetChange assetChange = element as AssetChange;
            yield return CoroutineUtils.StartCoroutine(ItemService.AssetChangeRoutine(assetChange));
        }
        yield return null;
    }
    
    public static void PositionInits(List<PositionChange> positionChanges){
        foreach(PositionChange positionChange in positionChanges){
            CharacterService.InstantiateCharacter(positionChange.CharacterID, positionChange.PositionID);
        }
    }
}
