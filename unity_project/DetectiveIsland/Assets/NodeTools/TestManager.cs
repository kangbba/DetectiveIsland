using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public List<Node> nodes;

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
        savedScenario = null;
        loadedScenario = null;
    }

    [ArokaButton]
    public void AddDialogueNode()
    {
        DialogueNode dialogueNode = new DialogueNode(Vector3.zero, 0, 0, "Dialogue");
        dialogueNode.dialogue = new Dialogue("Kate", new List<Line> { new Line("Smile", "Hi"), new Line("Smile", "Hello"), new Line("Smile", "¾È³ç") });
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

