using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Aroka.Anim;
using Microsoft.Unity.VisualStudio.Editor;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Linq;

public class DialoguePanel : MonoBehaviour
{
    [SerializeField] private ArokaAnimParent _arokaAnimParent;
    [SerializeField] private TextMeshProUGUI _characterText;
    [SerializeField] private TextMeshProUGUI _lineText;
    [SerializeField] private DialogueArrow _dialogueArrow;
    [SerializeField] private Button _exitBtn;

    private enum DialogueState { None, Typing, WaitingForNextSentence, Exit }
    private DialogueState _currentState;

    private void Start(){
        ClearPanel();
        _dialogueArrow.HideDialogueArrow();
        _exitBtn.gameObject.SetActive(false);
        _exitBtn.onClick.AddListener(OnExitButtonClick);
        SetState(DialogueState.None);
    }

    private void SetState(DialogueState newState)
    {
        _currentState = newState;
        switch (_currentState)
        {
            case DialogueState.None:
                // Any initialization for None state
                break;

            case DialogueState.Typing:
                _dialogueArrow.gameObject.SetActive(false);
                break;

            case DialogueState.WaitingForNextSentence:
                _dialogueArrow.gameObject.SetActive(true);
                break;

            case DialogueState.Exit:
                _exitBtn.gameObject.SetActive(false);
                break;
        }
    }

    private void OnExitButtonClick()
    {
        if (_currentState == DialogueState.WaitingForNextSentence)
        {
            SetState(DialogueState.Exit);
        }
    }

    public void ClearPanel(){
        _characterText.SetText("");
        _lineText.SetText("");
    }

    public void SetCharacterText(string s, Color c){
        _characterText.color = c;
        _characterText.SetText(s);
    }
   
    public async UniTask TypeDialogueTask(Dialogue dialogue){

        ECharacterID characterID = dialogue.CharacterID;
        bool isRyan = characterID == ECharacterID.Ryan;
        bool isMono = characterID == ECharacterID.Mono;
        CharacterData characterData = CharacterService.GetCharacterData(characterID);
        string[] delimiters = { @"\.", @"\,", @"\!", @"\?", @"\.\.\." }; // 구분할 문자열 패턴 정의
        
        foreach (var line in dialogue.Lines)
        {
            UIManager.ClearDialoguePanel();
            UIManager.SetDialogueCharacterText(characterData.CharacterNameForUser, characterData.CharacterColor);
            if (!(isRyan || isMono)){
                CharacterService.SetCharacterEmotion(characterID, line.EmotionID, .3f);
                CharacterService.StartCharacterTalking(characterID);
            }
            
            SetState(DialogueState.Typing);
            await TypeLineTask(line.Sentence.Trim(), Color.white); // 문장 출력
            CharacterService.StopCharacterTalking(characterID);
            SetState(DialogueState.WaitingForNextSentence);

            await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0)); // 마우스 클릭 대기
        }
    }

    public async UniTask WaitForDialogueExitButton()
    {
        _exitBtn.gameObject.SetActive(true);
        await UniTask.WaitUntil(() => _currentState == DialogueState.Exit);
        _exitBtn.gameObject.SetActive(false);
    }

    // 문장 출력을 위한 코루틴
    private async UniTask TypeLineTask(string str, Color c)
    {
        _dialogueArrow.HideDialogueArrow();
        _lineText.text = string.Empty;
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
        _dialogueArrow.ShowDialogueArrow(_lineText.GetPreferredValues());
    }

    public void OpenPanel(float totalTime){
        ClearPanel();
        _arokaAnimParent.SetOnAllChildren(true, totalTime);
    }

    public void ClosePanel(float totalTime){
        _arokaAnimParent.SetOnAllChildren(false, totalTime);
    }
}
