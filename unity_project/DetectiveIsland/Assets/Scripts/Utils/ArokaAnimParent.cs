using System.Collections.Generic;
using System.Linq;
using Aroka.Anim;
using Aroka.ArokaUtils;
using Aroka.EaseUtils;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ArokaAnimParent : MonoBehaviour
{
    [SerializeField] private ArokaAnim[] _arokAnimsToExclude;
    private ArokaAnim[] _arokaAnims;

    [ArokaButton]
    public void PreviewOn(){
        _arokaAnims =  GetComponentsInChildren<ArokaAnim>(false);
        for(int i = 0 ; i < _arokaAnims.Length ; i++){
            ArokaAnim arokaAnim = _arokaAnims[i];
            if(!_arokAnimsToExclude.Contains(arokaAnim)){
                 arokaAnim.EditorPreview_On();
            }
        }
    }
    [ArokaButton]
    public void PreviewOff(){
        _arokaAnims =  GetComponentsInChildren<ArokaAnim>(false);
        for(int i = 0 ; i < _arokaAnims.Length ; i++){
            ArokaAnim arokaAnim = _arokaAnims[i];
            if(!_arokAnimsToExclude.Contains(arokaAnim)){
                arokaAnim.EditorPreview_Off();
            }
        }
    }

    public void SetOnAllChildren(bool b, float totalTime){
        _arokaAnims =  GetComponentsInChildren<ArokaAnim>(false);
        for(int i = 0 ; i < _arokaAnims.Length ; i++){
            ArokaAnim arokaAnim = _arokaAnims[i];
            if(!_arokAnimsToExclude.Contains(arokaAnim)){
                _arokaAnims.SetAnims(b, totalTime);
            }
        }
    }
}

