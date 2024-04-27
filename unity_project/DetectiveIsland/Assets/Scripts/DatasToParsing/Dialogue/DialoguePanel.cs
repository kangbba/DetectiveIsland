using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Aroka.Anim;
using Microsoft.Unity.VisualStudio.Editor;
using Cysharp.Threading.Tasks;
public interface IDialoguePanel
{
    void Initialize();
    void ClearPanel();
    void SetAnim(bool visible, float duration);
    void SetCharacterText(string characterName, Color characterColor);
    UniTask TypeLineTask(string sentence, Color color);
}

public class DialoguePanel : MonoBehaviour, IDialoguePanel
{
    [SerializeField] private ArokaAnimParent _arokaAnimParent;
    [SerializeField] private TextMeshProUGUI _characterText;
    [SerializeField] private TextMeshProUGUI _lineText;

    [SerializeField] private DialogueArrow _dialogueArrow;
    
    public void Initialize(){
        ClearPanel();
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
    public async UniTask TypeLineTask(string str, Color c)
    {
        _lineText.text = "";
        _lineText.color = c;
        _dialogueArrow.gameObject.SetActive(false);
        foreach (char letter in str.ToCharArray())
        {
            _lineText.text += letter;
            if(letter == '!'){
                CameraController.ShakeCamera(3f, .3f);
            }
            if (letter != ' ') // 공백이 아닌 경우에만 대기하지 않음
                await UniTask.WaitForSeconds(0.06f);
        }
        _dialogueArrow.gameObject.SetActive(true);
        _dialogueArrow.SetAnchordPos(_lineText.GetPreferredValues());
    }

    public void SetAnim(bool visible, float duration)
    {
        _arokaAnimParent.SetOnAllChildren(visible, duration);
    }
}
