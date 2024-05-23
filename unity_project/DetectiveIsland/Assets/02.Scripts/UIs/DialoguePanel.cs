using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Aroka.Anim;
using Microsoft.Unity.VisualStudio.Editor;
using Cysharp.Threading.Tasks;

public class DialoguePanel : MonoBehaviour
{
    [SerializeField] private ArokaAnimParent _arokaAnimParent;
    [SerializeField] private TextMeshProUGUI _characterText;
    [SerializeField] private TextMeshProUGUI _lineText;

    [SerializeField] private DialogueArrow _dialogueArrow;
    
    private void Start(){
        ClearPanel();
        HideDialogueArrow();
    }
    public void ShowDialogueArrow(){
        _dialogueArrow.gameObject.SetActive(true);
    }

    public void HideDialogueArrow(){
        _dialogueArrow.gameObject.SetActive(false);
    }
    
    public void ClearPanel(){
        _characterText.SetText("");
        _lineText.SetText("");
    }
    public void SetCharacterText(string s, Color c){
        _characterText.color = c;
        _characterText.SetText(s);
    }
   
    // 문장 출력을 위한 코루틴
    public async UniTask TypeDialogueTask(string str, Color c)
    {
        _dialogueArrow.gameObject.SetActive(false);
        _lineText.text += ' ';
        _lineText.color = c;
        char[] characters = str.ToCharArray();
        
        for (int i = 0; i < characters.Length; i++)
        {
            char letter = characters[i];
            _lineText.text += letter;
            if (letter == '!')
            {
                CameraController.ShakeCamera(3f, .3f);
            }

            if (letter != ' ') // 공백이 아닌 경우에만 대기
                await UniTask.WaitForSeconds(DevelopmentTool.IsDebug ? 0f : 0.04f);
        }
        _dialogueArrow.SetAnchordPos(_lineText.GetPreferredValues());
        _dialogueArrow.gameObject.SetActive(true);
    }
    public void OpenPanel(float totalTime){
        _arokaAnimParent.SetOnAllChildren(true, totalTime);
    }
    public void ClosePanel(float totalTime){
        _arokaAnimParent.SetOnAllChildren(false, totalTime);
    }
}
