using System;
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

        var targetObject = target;
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

                        // Set the target object as dirty to make sure changes are saved
                        if (!Application.isPlaying)
                        {
                            EditorUtility.SetDirty(targetObject);
                            PrefabUtility.RecordPrefabInstancePropertyModifications(targetObject);
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
        // Here you can add additional checks, for now it's always true
        return true;
    }
}
