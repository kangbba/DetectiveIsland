using Aroka.JsonUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public List<Node> nodes = new List<Node>();

    [ArokaButton]
    public void Save()
    {
        
        savedScenario = new Scenario(null,nodes.ToElements());

        Debug.Log(nodes.ToElements().Count);

        ArokaJsonUtils.SaveScenario(savedScenario, "TestJson");
    }


    public Scenario savedScenario;
    public Scenario loadedScenario;


    [ArokaButton]
    public void Load()
    {
        loadedScenario = ArokaJsonUtils.LoadScenario("TestJson");
        if(loadedScenario == null){
            Debug.LogWarning("loadedScenario is null");
            return;
        }
        Debug.Log(loadedScenario.Elements.Count);
    }


    [ArokaButton]
    public void ClearNodes()
    {
        nodes.Clear();
        nodes = new List<Node>();
        savedScenario = null;
        loadedScenario = null;
    }

    [ArokaButton]
    public void AddDialogueNode()
    {
        DialogueNode dialogueNode = new DialogueNode(0,0, "Dialogue");
        //이건 주석이야 히히 ㅎㅎ !! 호호 ㅋㅋ ㄷㄷ
        dialogueNode.dialogue = new Dialogue("Kate", new List<Line> { new Line("Smile", "Hi"), new Line("Smile", "Hello"), new Line("Smile", "뭐라뭐라 어쩌고 !") });
        //괜히 주석달기 뷁 쉚
        Debug.Log(dialogueNode.dialogue);
        nodes.Add(dialogueNode);
    }
    [ArokaButton]
    public void AddChoiceSetNode()
    {
        ChoiceSetNode choiceSetNode = new ChoiceSetNode(new Rect(), "ChoiceSetNode");
        Dialogue dialogue = new Dialogue("Kate", new List<Line> { new Line("Smile", "뭐를 먹고싶어?"), new Line("Smile", "골라봐")});

        Dialogue dialogue1 = new Dialogue("Kate", new List<Line> { new Line("Smile", "선택지 1을 골랐구나"), new Line("Smile", "그거아주 좋은선택이야"), new Line("Smile", "잘했어") });
        
        Dialogue dialogue2 = new Dialogue("Kate", new List<Line> { new Line("Smile", "선택지 2을 골랐구나") });
        Dialogue dialogue21 = new Dialogue("Mono", new List<Line> { new Line("Smile", "역시 이게 맞는선택이라고 생각했어"), new Line("Smile", "후훗") });
        
        Dialogue dialogue3 = new Dialogue("Kate", new List<Line> { new Line("Smile", "선택지 3을 골랐구나"), new Line("Smile", "...") });
        Dialogue dialogue31 = new Dialogue("Mono", new List<Line> { new Line("Smile", "너 왜 말이없니")});


        ChoiceSet choiceSet = new ChoiceSet
        (
            new List<Dialogue>() { dialogue, },

            new List<Choice>() { 
                new Choice("선택지 1", new List<Element>() { dialogue1 }),
                new Choice("선택지 2" , new List<Element>() { dialogue2, dialogue21 }),
                new Choice("선택지 3" , new List<Element>() {  dialogue3, dialogue31 })
            }

        );

        choiceSetNode.choiceSet = choiceSet;
        nodes.Add(choiceSetNode);
    }
    [ArokaButton]
    public void AddAssetChangeNode()
    {
        ChoiceSetNode choiceSetNode = new ChoiceSetNode(new Rect(), "ChoiceSetNode");
        Dialogue dialogue = new Dialogue("Kate", new List<Line> { new Line("Smile", "뭐를 먹고싶어?"), new Line("Smile", "골라봐")});

        Dialogue dialogue1 = new Dialogue("Kate", new List<Line> { new Line("Smile", "선택지 1을 골랐구나"), new Line("Smile", "그거아주 좋은선택이야"), new Line("Smile", "잘했어") });
        
        Dialogue dialogue2 = new Dialogue("Kate", new List<Line> { new Line("Smile", "선택지 2을 골랐구나") });
        Dialogue dialogue21 = new Dialogue("Mono", new List<Line> { new Line("Smile", "역시 이게 맞는선택이라고 생각했어"), new Line("Smile", "후훗") });
        
        Dialogue dialogue3 = new Dialogue("Kate", new List<Line> { new Line("Smile", "선택지 3을 골랐구나"), new Line("Smile", "...") });
        Dialogue dialogue31 = new Dialogue("Mono", new List<Line> { new Line("Smile", "너 왜 말이없니")});


        ChoiceSet choiceSet = new ChoiceSet
        (
            new List<Dialogue>() { dialogue, },

            new List<Choice>() { 
                new Choice("선택지 1", new List<Element>() { dialogue1 }),
                new Choice("선택지 2" , new List<Element>() { dialogue2, dialogue21 }),
                new Choice("선택지 3" , new List<Element>() {  dialogue3, dialogue31 })
            }

        );
        
        AssetChangeNode assetChangeNode = new AssetChangeNode(new Rect(), "AssetChangeNode");
        nodes.Add(assetChangeNode);
    }
    [ArokaButton]
    public void AddItemDemandNode()
    {
        
        ItemDemandNode itemDemandNode = new ItemDemandNode(new Rect(), "ItemDemandNode");
        Dialogue dialogue = new Dialogue("Kate", new List<Line> { new Line("Smile", "맞을때까지 골라봐") });
        Dialogue dialogue1 = new Dialogue("Kate", new List<Line> { new Line("Smile", "이건 성공했을때 하는 반응이야"), new Line("Smile", "성공해서 축하해")});
        Dialogue dialogue2 = new Dialogue("Kate", new List<Line> { new Line("Smile", "실망스럽구나"), new Line("Smile", "하지만 포기는 하지마..")});

        ItemDemand itemDemand = new ItemDemand
        (
            itemID : "note_pad",
            dialogues : new List<Dialogue>() { dialogue, },
            successElements : new List<Element>() {  dialogue1} ,
            failElements : new List<Element>() {  dialogue2 } 
        );
        itemDemandNode.itemDemand = itemDemand;
        nodes.Add(itemDemandNode);
    }

    [ArokaButton]
    public void AddPositionChangeNode()
    {
        PositionChangeNode positionChangeNode = new PositionChangeNode(new Rect(), "PositionChangeNode");
        nodes.Add(positionChangeNode); 
    }

/*






*/




    [ArokaButton]
    public void Test()
    {


    }
}

