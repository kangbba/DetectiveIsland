using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class MonoBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var targetObject = serializedObject.targetObject;
        var methods = targetObject.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var method in methods)
        {
            var attribute = method.GetCustomAttribute<ArokaButtonAttribute>();
            if (attribute != null)
            {
                if (GUILayout.Button(method.Name))
                {
                    if (attribute.PerformCheck(targetObject))
                    {
                        method.Invoke(targetObject, null);
                        if(EditorUtility.IsDirty(target)){  
                            EditorUtility.SetDirty(target);
                            SceneView.RepaintAll();
                        }
                    }
                    else
                    {
                        Debug.LogError(attribute.Error);
                    }
      
                }
            }
        }
    }
}

public class ArokaButtonAttribute : Attribute
{
    public string Error { get; set; } = "Cannot execute this function.";

    public bool PerformCheck(UnityEngine.Object obj)
    {
       // return !PrefabUtility.IsPartOfPrefabAsset(obj);
        return true;
    }
}


public static class PrefabUtility
{
    public static bool IsPartOfPrefabAsset(UnityEngine.Object obj)
    {
        return UnityEditor.PrefabUtility.GetPrefabAssetType(obj) != UnityEditor.PrefabAssetType.NotAPrefab;
    }
}

