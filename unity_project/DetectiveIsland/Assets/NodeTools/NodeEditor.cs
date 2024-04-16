using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Formatting = Newtonsoft.Json.Formatting;
public static class StoragePath
{
    public static string ScenarioPath => Application.dataPath;

}


public static class ArokaJsonUtil
{
    public static void SaveScenario(Scenario scenario, string fileName)
    {
        string fullPath = Path.Combine(StoragePath.ScenarioPath, fileName + ".json");

        if (File.Exists(fullPath))
        {
            bool overwrite = EditorUtility.DisplayDialog(
                "똑같은 파일이 존재합니다. 진짜 덮어쓰시겠습니까?? 기존 파일은 삭제됩니다.",
                "A file already exists at " + fullPath + ". Do you want to overwrite it?",
                "네",
                "취소"
            );

            if (!overwrite)
            {
                Debug.Log("File save cancelled.");
                return;
            }
        }

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented
        };

        string json = JsonConvert.SerializeObject(scenario, settings);
        File.WriteAllText(fullPath, json);
        AssetDatabase.Refresh(); // Refresh the Asset Database to include the new file.
        Debug.Log("File saved: " + fullPath);
    }

    public static Scenario LoadScenario(string fileName)
    {
        string fullPath = Path.Combine(StoragePath.ScenarioPath, fileName + ".json");

        if (!File.Exists(fullPath))
        {
            Debug.LogError("File not found: " + fullPath);
            return null;
        }

        string json = File.ReadAllText(fullPath);
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto // Ensure that type information is handled correctly
        };

        Scenario scenario = JsonConvert.DeserializeObject<Scenario>(json, settings);

        if (scenario != null && scenario.Elements != null)
        {
            Debug.Log("Load Complete, elements Count = " + scenario.Elements.Count);
            foreach (Element element in scenario.Elements)
            {
                if (element is Dialogue dialogue)
                {
                    Debug.Log($"Dialogue Element: CharacterID={dialogue.CharacterID}, Lines Count={dialogue.Lines.Count}");
                    foreach (var line in dialogue.Lines)
                    {
                        Debug.Log($"Line: Emotion={line.EmotionID}, Text={line.Sentence}");
                    }
                }
                else
                {
                    Debug.Log($"Unknown Element Type: {element.GetType().Name}");
                }
            }
        }
        else
        {
            Debug.LogError("Failed to deserialize the JSON content into a Scenario object or the Elements list is null.");
        }
        return scenario;
    }
}

public static class NodeService
{
    public static List<Element> ToElements(this List<Node> nodes)
    {

        List<Element> list = new List<Element>();

        for (int i = 0; i < nodes.Count; i++)
        {
            Node node = nodes[i];
            list.Add(node.ToProperElement());
        }
        return list;
    }

    public static Element ToProperElement(this Node node)
    {
        if (node is DialogueNode dialogueNode)
        {
            return dialogueNode.dialogue;
        }
        else if (node is ChoiceSetNode choiceSetNode)
        {
            return choiceSetNode.choiceSet;
        }
        else if (node is ItemDemandNode itemDemandNode)
        {
            return itemDemandNode.itemDemand;
        }
        else if (node is PositionChangeNode positionChangeNode)
        {
            return positionChangeNode.positionChange;
        }
        else if (node is AssetChangeNode assetChangeNode)
        {
            return assetChangeNode.assetChange; 
        }
        return null; 
    }



}

public class NodeEditor : EditorWindow
{
    private List<Node> nodes;
    private Node selectedNode;
    private Vector2 mousePosition;
    private Vector2 lastMouseDragPosition; // Track the last position of the mouse during a drag
    private Vector2 canvasOffset;
    private bool isDraggingNode;
    private bool isPanning;

    [MenuItem("JNode/Create Json Node")]
    private static void OpenWindow()
    {
        NodeEditor window = GetWindow<NodeEditor>();
        window.titleContent = new GUIContent("JNode Editor");
    }

    private void OnEnable()
    {
        nodes = new List<Node>();
        canvasOffset = Vector2.zero;
    }

    private void OnGUI()
    {
        ProcessEvents(Event.current);
        DrawNodes();

        if (isDraggingNode && selectedNode != null)
        {
            selectedNode.rect.position = mousePosition - selectedNode.dragOffset + canvasOffset;
            Repaint();
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Nodes to JSON"))
        {
            SaveCurrentNodes();
        }
        GUILayout.EndHorizontal();
    }

    private void ProcessEvents(Event e)
    {
        mousePosition = e.mousePosition;
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 2) // Middle mouse button
                {
                    isPanning = true;
                    lastMouseDragPosition = e.mousePosition;
                    e.Use();
                }
                else if (e.button == 1) // Right click
                {
                    ProcessContextMenu();
                    e.Use();
                }
                else if (e.button == 0) // Left click
                {
                    Node node = GetNodeAtPosition(mousePosition - canvasOffset);
                    if (node != null)
                    {
                        selectedNode = node;
                        if (e.clickCount == 2) // Double click
                        {
                            e.Use();
                        }
                        else
                        {
                            isDraggingNode = true;
                            selectedNode.dragOffset = mousePosition - selectedNode.rect.position;
                            e.Use();
                        }
                    }
                    else
                    {
                        selectedNode = null; // Clear selection if click outside any node
                        isDraggingNode = false;
                    }
                }
                break;
            case EventType.MouseDrag:
                if (isPanning && e.button == 2)
                {
                    Vector2 delta = e.mousePosition - lastMouseDragPosition;
                    canvasOffset += delta;
                    lastMouseDragPosition = e.mousePosition;
                    Repaint();
                }
                break;
            case EventType.MouseUp:
                if (e.button == 2)
                    isPanning = false;
                isDraggingNode = false;
                break;
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.Delete && selectedNode != null)
                {
                    nodes.Remove(selectedNode);
                    selectedNode = null;
                    Repaint();
                }
                break;
        }
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("New Dialogue"), false, () => OnClickAddNode(mousePosition - canvasOffset));
        if (selectedNode != null)
        {
            genericMenu.AddItem(new GUIContent("Delete Node"), false, () => DeleteSelectedNode());
        }
        genericMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 position)
    {
        DialogueNode dialogueNode = new DialogueNode(position, 200, 100, "Dialogue");
        nodes.Add(dialogueNode);
    }

    private void DeleteSelectedNode()
    {
        if (selectedNode != null)
        {
            nodes.Remove(selectedNode);
            selectedNode = null;
            Repaint();
        }
    }

    private void DrawNodes()
    {
        foreach (var node in nodes)
        {
            Rect adjustedRect = new Rect(node.rect.position + canvasOffset, node.rect.size);
            node.rect = adjustedRect;
            node.DrawNode();
            node.rect.position -= canvasOffset; // Reset the node's rect for consistent logical operations
        }
        if (selectedNode != null)
        {
            // Highlight the selected node
            GUI.color = Color.cyan;
            GUI.Box(selectedNode.rect, "", EditorStyles.helpBox);
            GUI.color = Color.white;
        }
    } 

    private Node GetNodeAtPosition(Vector2 position)
    {
        foreach (var node in nodes)
        {
            if (node.rect.Contains(position))
            {
                return node;
            }
        }
        return null;
    }

    private void SaveCurrentNodes()
    {
        if (nodes.Count > 0)
        {
            JsonUtilityHelper.SaveNodesToJson(nodes, "NodesData.json");
            Debug.Log("Nodes saved to JSON.");
        }
        else
        {
            Debug.Log("No nodes to save.");
        }
    }
}
public static class JsonUtilityHelper
{
    public static void SaveNodesToJson(List<Node> nodes, string fileName)
    {
        string path = Path.Combine(Application.dataPath, fileName);
        string json = JsonUtility.ToJson(new Serialization<Node>(nodes), true);
        File.WriteAllText(path, json);
        AssetDatabase.Refresh();
    }

    [System.Serializable]
    private class Serialization<T>
    {
        public List<T> items;
        public Serialization(List<T> items)
        {
            this.items = items;
        }
    }
}