using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
public static class ChoiceSetUI
{
    private static ChoiceSetPanel _choiceSetPanel;

    public static ChoiceSetPanel ChoiceSetPanel { get => _choiceSetPanel; }

    public static void Load(){
        _choiceSetPanel = UIManager.Instance.ChoiceSetPanel; 
    }


}
