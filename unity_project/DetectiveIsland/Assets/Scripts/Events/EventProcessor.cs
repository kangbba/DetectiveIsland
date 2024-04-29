using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;

public static class EventProcessor
{
    //시나리오 Task 구조
    public static async UniTask ScenarioTask(Scenario scenario){

        DialogueUI.DialoguePanel.ClearPanel();
        DialogueUI.DialoguePanel.OpenPanel(1f);
        await UniTask.WaitForSeconds(1f);
        //대화창 On
        
        //Elements 출력
        List<Element> elements = scenario.Elements;
        await ProcessElementsTask(elements);

        DialogueUI.DialoguePanel.ClosePanel(1f);
        await UniTask.WaitForSeconds(1f);

    }

    private static async UniTask ProcessElementsTask(List<Element> elements){

        foreach(Element element in elements){
            await ProcessElementTask(element);
        }
    }
    
    public static async UniTask ProcessElementTask(Element element){

        Debug.Log(element.GetType());
        if(element is PositionInit)
        {
            PositionInit positionInit = element as PositionInit;
            await ProcessPositionInit(positionInit);
        }
        else if(element is Dialogue)
        {
            Dialogue dialogue = element as Dialogue;
            await ProcessDialogue(dialogue);
        }
        else if(element is ChoiceSet)
        {
            ChoiceSet choiceSet = element as ChoiceSet;
            await ProcessChoiceSet(choiceSet);

        }
        else if(element is ItemDemand)
        {
            ItemDemand itemDemand = element as ItemDemand;
            await ProcessItemDemand(itemDemand);
        }
        else if(element is AssetChange)
        {
            AssetChange assetChange = element as AssetChange;
            await ItemService.AssetChangeTask(assetChange);
        }
    }
    
    public static async UniTask ProcessPositionInit(PositionInit positionInit){
        foreach(CharacterPosition characterPosition in positionInit.CharacterPositions){
            Character instancedCharacter = CharacterService.GetInstancedCharacter(characterPosition.CharacterID);
            bool isPositionChangeOnly = instancedCharacter != null;
            if(isPositionChangeOnly){
                instancedCharacter.SetPos(characterPosition.PositionID, 1f);
            }
            else{
               CharacterService.FadeOutCharacterThenDestroy(characterPosition.CharacterID, .3f);
               CharacterService.InstantiateCharacterThenFadeIn(characterPosition.CharacterID, characterPosition.PositionID, "Smile", 1f);
            }

        }
        await UniTask.WaitForSeconds(1f);
    }

    public static async UniTask ProcessChoiceSet(ChoiceSet choiceSet){
        ChoiceSetPanel choiceSetPanel = ChoiceSetUI.ChoiceSetPanel;
        foreach(Dialogue dialogue in choiceSet.Dialogues){
            await ProcessDialogue(dialogue);
        }
        Choice selectedChoice = await choiceSetPanel.MakeChoiceBtnsAndWait(choiceSet);
        await ProcessElementsTask(selectedChoice.Elements);
    }

    public static async UniTask ProcessDialogue(Dialogue dialogue)
    {
        string characterID = dialogue.CharacterID;
        CharacterData characterData = CharacterService.GetCharacterData(characterID);
        Character instancedCharacter = CharacterService.GetInstancedCharacter(characterID);
        DialoguePanel dialoguePanel = DialogueUI.DialoguePanel;
        if (instancedCharacter != null)
        {
            CameraController.MoveX(instancedCharacter.transform.position.x / 10f, 1f);
        }

        foreach (var line in dialogue.Lines)
        {
            if (instancedCharacter != null)
            {
                instancedCharacter.SetEmotion(line.EmotionID, .3f);
                instancedCharacter.StartTalking();
            }

            dialoguePanel.SetCharacterText(characterData.CharacterNameForUser, characterData.CharacterColor);
            await dialoguePanel.TypeLineTask(line.Sentence, Color.white);

            if (instancedCharacter != null)
            {
                instancedCharacter.StopTalking();
            }

            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
        }
    }

    public static async UniTask ProcessItemDemand(ItemDemand itemDemand)
    {
        foreach(Dialogue dialogue in itemDemand.Dialogues){
            await ProcessDialogue(dialogue);
        }

        while(true){
            ItemUI.ItemDemandPanel.OpenPanel(); 

            ItemData selectedItemData = await ItemUI.ItemDemandPanel.OpenItemDemandPanelAndWait();
            Debug.Log($"{selectedItemData.ItemNameForUser}을 골랐다!");

            ItemUI.ItemDemandPanel.ClosePanel(); 

            if (selectedItemData == null)
            {
                Debug.Log("취소 되었음.");
                await ProcessElementsTask(itemDemand.CancelElements);
                break;
            }
            else if (selectedItemData.ItemID == itemDemand.ItemID)
            {
                Debug.Log($"{selectedItemData.ItemNameForUser}을 골랐다!");
                Debug.Log("정답이므로 elements 처리 후 이 루틴을 빠져나갈 예정");
                await ProcessElementsTask(itemDemand.SuccessElements);
                break;
            }
            else
            {
                Debug.Log("오답이므로 elements 처리 후 이 루틴이 반복될 예정");
                await ProcessElementsTask(itemDemand.FailElements);
            }
        }
    }

    public static PositionInit GetFirstPositionInit(Scenario scenario)
    {
        return scenario.Elements.FirstOrDefault( element => element is PositionInit ) as PositionInit;
    }

    public static PositionInit GetLastPositionInit(Scenario scenario)
    {
        return scenario.Elements.LastOrDefault( element => element is PositionInit ) as PositionInit;
    }
}
