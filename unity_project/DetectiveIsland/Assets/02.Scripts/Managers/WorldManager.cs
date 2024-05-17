using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WorldManager
{
    private static Transform _garbagePanel;
    public static Transform GarbagePanel { get => _garbagePanel; }




    public static void Load(){
        _garbagePanel = new GameObject("Garbage Panel").transform;
    }


    
}
