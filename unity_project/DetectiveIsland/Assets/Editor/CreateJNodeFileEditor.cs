using UnityEditor;
using UnityEngine;
using System.IO;

public class CreateJNodeFileEditor
{
    [MenuItem("Assets/Create/New JNode", false, 80)]
    public static void CreateNewJNode()
    {
        string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (!Directory.Exists(folderPath))
        {
            folderPath = Path.GetDirectoryName(folderPath);
        }

        string path = AssetDatabase.GenerateUniqueAssetPath(folderPath + "/NewJNode.jnode");

        File.WriteAllText(path, "{}"); // Creates an empty JSON object in the file.
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(path);
    }
}