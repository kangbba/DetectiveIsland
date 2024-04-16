using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialoguePanel : ArokaAnim
{
    public TextMeshProUGUI _characterText;
    public TextMeshProUGUI _lineText;
    
    public void Initialize(){
        _characterText.SetText("");
        _lineText.SetText("");
    } 

    public void SetCharacterText(string s, Color c){
        _characterText.color = c;
        _characterText.SetText(s);
    }
    // 문장 출력을 위한 코루틴
    public IEnumerator TypeLineRoutine(string str, Color c)
    {
        _lineText.text = "";
        _lineText.color = c;
        foreach (char letter in str.ToCharArray())
        {
            _lineText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

}
