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
    public static Scenario LoadScenario(TextAsset jsonTextAsset)
    {
        if (jsonTextAsset == null)
        {
            Debug.LogError("No TextAsset provided.");
            return null;
        }

        return NewToneJsonConverterExtension.ConvertFromTextAsset<Scenario>(jsonTextAsset, NewToneJsonConverterExtension.JsonSerializerSettings_MaxDetail);
    }

    public static Scenario LoadScenario(string filePath)
    {
        return NewToneJsonConverterExtension.ConvertFromPath<Scenario>(filePath, NewToneJsonConverterExtension.JsonSerializerSettings_MaxDetail);
    }
}