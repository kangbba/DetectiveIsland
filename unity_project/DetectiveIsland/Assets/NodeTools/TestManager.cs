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

        savedScenario = new Scenario(nodes.ToElements());

        Debug.Log(nodes.ToElements().Count);

        ArokaJsonUtil.SaveScenario(savedScenario, "TestJson");
    }


    public Scenario savedScenario;
    public Scenario loadedScenario;


    [ArokaButton]
    public void Load()
    {
        loadedScenario = ArokaJsonUtil.LoadScenario("TestJson");
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
        DialogueNode dialogueNode = new DialogueNode(Vector3.zero, 0, 0, "Dialogue");
        //이건 주석이야 히히 ㅎㅎ !! 호호 ㅋㅋ ㄷㄷ
        dialogueNode.dialogue = new Dialogue("Kate", new List<Line> { new Line("Smile", "Hi"), new Line("Smile", "Hello"), new Line("Smile", "뭐라뭐라 어쩌고 !") });
        //괜히 주석달기 뷁 쉚
        Debug.Log(dialogueNode.dialogue);
        nodes.Add(dialogueNode);
    }


    [ArokaButton]
    public void AddAssetChangeNode()
    {
        AssetChangeNode assetChangeNode = new AssetChangeNode(Vector3.zero, 0, 0, "AssetChangeNode");
        nodes.Add(assetChangeNode);
    }

    [ArokaButton]
    public void AddItemDemandNode()
    {
        ItemDemandNode itemDemandNode = new ItemDemandNode(Vector3.zero, 0, 0, "ItemDemandNode");
        nodes.Add(itemDemandNode);
    }

    [ArokaButton]
    public void AddPositionChangeNode()
    {
        PositionChangeNode positionChangeNode = new PositionChangeNode(Vector3.zero, 0, 0, "PositionChangeNode");
        nodes.Add(positionChangeNode);
    }

    [ArokaButton]
    public void AddChoiceSetNode()
    {
        ChoiceSetNode choiceSetNode = new ChoiceSetNode(Vector3.zero, 0, 0, "ChoiceSetNode");
        nodes.Add(choiceSetNode);
    }





    [ArokaButton]
    public void Test()
    {


    }
}

