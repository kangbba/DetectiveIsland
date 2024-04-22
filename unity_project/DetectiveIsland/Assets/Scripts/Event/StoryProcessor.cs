using System.Collections;
using System.Collections.Generic;
using Aroka.CoroutineUtils;
using Aroka.JsonUtils;
using UnityEngine;

public static class StoryProcessor
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

        if(element is PositionChange){
            PositionChange positionChange = element as PositionChange;
            CharacterService.PositionChange(positionChange.CharacterID, positionChange.PositionID, 1f);
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
            while(true){
                ItemData selectedItemData = null;
                yield return CoroutineUtils.AwaitCoroutine<ItemData>(ItemService.GetSelectedItemFromItemDemand(itemDemand), result => {
                    selectedItemData = result;
                });
                if(selectedItemData == null){
                    Debug.Log("취소 버튼을 눌렀으므로 일단 이 루프를 빠져나갈 예정");
                    yield return CoroutineUtils.StartCoroutine(ProcessElementsRoutine(itemDemand.FailElements));
                    break;
                }
                else if(selectedItemData.ItemID == itemDemand.ItemID){
                    Debug.Log("정답이므로 elements 처리후 이 루프를 빠져나갈 예정");
                    yield return CoroutineUtils.StartCoroutine(ProcessElementsRoutine(itemDemand.SuccessElements));
                    break;
                }
                else{
                    Debug.Log("오답이므로 elements 처리후 이 루프가 반복될 예정");
                    yield return CoroutineUtils.StartCoroutine(ProcessElementsRoutine(itemDemand.FailElements));
                }
            }
        }
        else if(element is AssetChange){
            AssetChange assetChange = element as AssetChange;
            yield return CoroutineUtils.StartCoroutine(ItemService.AssetChangeRoutine(assetChange));
        }
        yield return null;
    }
    
}
