using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, "jnode")]
public class JNodeImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        // Create an asset representation for the custom file
        TextAsset textAsset = new TextAsset(System.IO.File.ReadAllText(ctx.assetPath));
        ctx.AddObjectToAsset("text", textAsset);
        ctx.SetMainObject(textAsset);
    }
}
