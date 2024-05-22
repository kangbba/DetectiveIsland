using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;
using Aroka.ArokaUtils;

public static class JNodeUtils
{
    public static void SaveJNode(JNode jNode, string path)
    {
        string json = JsonConvert.SerializeObject(jNode, NewToneJsonConverterExtension.JsonSerializerSettings_MaxDetail);
        File.WriteAllText(path, json);
        AssetDatabase.Refresh();
        Debug.Log("File saved: " + path);
    }

    public static JNode LoadJNode(string filePath)
    {
        return  NewToneJsonConverterExtension.ConvertJsonToClass_FromPath<JNode>(filePath, NewToneJsonConverterExtension.JsonSerializerSettings_MaxDetail);
    }

    public static void SaveScenario(Scenario scenario, string fileName)
    {
        string fullPath = Path.Combine(JNodePaths.ScenarioJsonFileSavePath, fileName + ".json");
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

        string json = NewToneJsonConverterExtension.ConvertClassToJson(scenario, NewToneJsonConverterExtension.JsonSerializerSettings_MaxDetail);
        File.WriteAllText(fullPath, json);
        AssetDatabase.Refresh();
        Debug.Log("File saved: " + fileName);
    }

}

