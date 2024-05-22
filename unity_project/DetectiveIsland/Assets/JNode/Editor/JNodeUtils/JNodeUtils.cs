using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;

public static class JNodeUtils
{
    static JsonSerializerSettings JnodeJsonSerializerSettings
    {
        get
        {
            return new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter> { new Vector2Converter() },
                StringEscapeHandling = StringEscapeHandling.Default // Ensuring Hangul is not escaped
            };
        }
    }

  
    public static void SaveJNode(JNode jNode, string path)
    {
        string json = JsonConvert.SerializeObject(jNode, JnodeJsonSerializerSettings);
        File.WriteAllText(path, json);
        AssetDatabase.Refresh();
        Debug.Log("File saved: " + path);
    }

    public static JNode LoadJNode(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return null;
        }
        string json = File.ReadAllText(filePath);

     
        return JsonConvert.DeserializeObject<JNode>(json, JnodeJsonSerializerSettings);
    }

    public static void SaveScenario(Scenario scenario, string fileName)
    {
        string fullPath = Path.Combine(StoragePath.ScenarioPath, fileName + ".json");
        if (File.Exists(fullPath))
        {
            bool overwrite = EditorUtility.DisplayDialog(
                "똑같은 파일이 존재합니다. 진짜 덮어쓰시겠습니까?? 기존 파일은 삭제됩니다.",
                $"A file already exists at {fileName} Do you want to overwrite it?",
                "네",
                "취소"
            );

            if (!overwrite)
            {
                Debug.Log("File save cancelled.");
                return;
            }
        }

        string json = JsonConvert.SerializeObject(scenario, JnodeJsonSerializerSettings);
        File.WriteAllText(fullPath, json);
        AssetDatabase.Refresh();
        Debug.Log("File saved: " + fileName);
    }

    // JSON 문자열을 역직렬화하여 Scenario 객체를 반환합니다.
    private static Scenario DeserializeScenario(string json)
    {
        Scenario scenario = JsonConvert.DeserializeObject<Scenario>(json, JnodeJsonSerializerSettings);
        return scenario;
    }

    public static void ScenarioLog(Scenario scenario)
    {
        if (scenario != null && scenario.Elements != null)
        {
            Debug.Log("///////////////////////////////////////////////////////////");
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
                }
            }
        }
        else
        {
            Debug.LogError("Failed to deserialize the JSON content into a Scenario object or the Elements list is null.");
        }
        Debug.Log("///////////////////////////////////////////////////////////");
    }
}



namespace Aroka.JsonUtils{
    public static class ArokaJsonUtils
    {
    }
}