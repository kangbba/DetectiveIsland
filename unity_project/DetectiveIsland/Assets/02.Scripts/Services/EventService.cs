using Aroka.ArokaUtils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public static class EventService
{   
    public static void Load(){
        
    }
    public static Scenario LoadScenario(this TextAsset jsonTextAsset)
    {
        if (jsonTextAsset == null)
        {
            Debug.LogWarning("No TextAsset provided.");
            return null;
        }

        return NewToneJsonConverterExtension.ConvertJsonToClass_FromJsonTextAsset<Scenario>(jsonTextAsset, NewToneJsonConverterExtension.JsonSerializerSettings_MaxDetail);
    }

    public static Scenario LoadScenario(string filePath)
    {
        return NewToneJsonConverterExtension.ConvertJsonToClass_FromPath<Scenario>(filePath, NewToneJsonConverterExtension.JsonSerializerSettings_MaxDetail);
    }
}