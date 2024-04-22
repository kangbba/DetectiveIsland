using System.Collections.Generic;
using Aroka.Anim;
using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ArokaAnimParent : MonoBehaviour
{
    [SerializeField] private bool _isContainSelf;
    [SerializeField] private ArokaAnim[] _arokaAnims;

    [ArokaButton]
    public void PreviewOn(){
        _arokaAnims =  GetComponentsInChildren<ArokaAnim>(_isContainSelf);
        for(int i = 0 ; i < _arokaAnims.Length ; i++){
            ArokaAnim arokaAnim = _arokaAnims[i];
            arokaAnim.EditorPreview_On();
        }
    }
    [ArokaButton]
    public void PreviewOff(){
        _arokaAnims =  GetComponentsInChildren<ArokaAnim>(_isContainSelf);
        for(int i = 0 ; i < _arokaAnims.Length ; i++){
            ArokaAnim arokaAnim = _arokaAnims[i];
            arokaAnim.EditorPreview_Off();
        }
    }

    public void SetOnAllChildren(bool b, float totalTime){
        _arokaAnims =  GetComponentsInChildren<ArokaAnim>(_isContainSelf);
        _arokaAnims.SetAnims(b, totalTime);
    }
}

