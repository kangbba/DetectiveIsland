using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;

namespace Aroka.JsonUtils{
    public static class ArokaJsonUtils
    {
        public static void SaveToJson<T>(T objectToSave, string filePath)
        {
            if (File.Exists(filePath))
            {
                bool overwrite = EditorUtility.DisplayDialog(
                    "똑같은 파일이 존재합니다. 진짜 덮어쓰시겠습니까?? 기존 파일은 삭제됩니다.",
                    $"A file already exists at {filePath}. Do you want to overwrite it?",
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
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented,
                StringEscapeHandling = StringEscapeHandling.Default // Ensuring Hangul is not escaped
            };

            string json = JsonConvert.SerializeObject(objectToSave, settings);
            File.WriteAllText(filePath, json);
            AssetDatabase.Refresh();
            Debug.Log("File saved: " + filePath);
        }

        public static void SaveScenario(Scenario scenario, string fillPath)
        {

            if (File.Exists(fillPath))
            {
                bool overwrite = EditorUtility.DisplayDialog(
                    "똑같은 파일이 존재합니다. 진짜 덮어쓰시겠습니까?? 기존 파일은 삭제됩니다.",
                    $"A file already exists at {fillPath} Do you want to overwrite it?",
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
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented,
                StringEscapeHandling = StringEscapeHandling.Default // Ensuring Hangul is not escaped
            };

            string json = JsonConvert.SerializeObject(scenario, settings);
            File.WriteAllText(fillPath, json);
            AssetDatabase.Refresh();
            Debug.Log("File saved: " + fillPath);
        }
        // 파일 경로를 통해 시나리오를 로드합니다.
        public static Scenario LoadScenario(string fileName)
        {
            string fullPath = Path.Combine(StoragePath.ScenarioPath, fileName + ".json");
            if (!File.Exists(fullPath))
            {
                Debug.LogError($"File not found: {fullPath}");
                return null;
            }
            string json = File.ReadAllText(fullPath);
            return DeserializeScenario(json);   
        }
        // TextAsset을 통해 시나리오를 로드합니다.
        public static Scenario LoadScenario(TextAsset jsonTextAsset)
        {
            if (jsonTextAsset == null)
            {
                Debug.LogError("No TextAsset provided.");
                return null;
            }

            return DeserializeScenario(jsonTextAsset.text);
        }
        // JSON 문자열을 역직렬화하여 Scenario 객체를 반환합니다.
        private static Scenario DeserializeScenario(string json)
        { 
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented,
                StringEscapeHandling = StringEscapeHandling.Default
            };
            Scenario scenario = JsonConvert.DeserializeObject<Scenario>(json, settings);
            ScenarioLog(scenario);
            return scenario;
        }

        public static void ScenarioLog(Scenario scenario){
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
                        Debug.Log($"Unknown Element Type: {element.GetType().Name}");
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
}