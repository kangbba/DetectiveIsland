using System.Collections;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Aroka.EaseUtils;

public class OverlaySentenceDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private CanvasGroup _canvasGroup;

    public async UniTask DisplayOverlaySentence(OverlaySentence overlaySentence)
    {
        string sentence = overlaySentence.Sentence;
        float sentenceTime = overlaySentence.SentenceTime;
        float afterDelayTime = overlaySentence.AfterDelayTime;
        _textMeshPro.text = "";

        _canvasGroup.EaseCanvasGroupAlpha(1f, 1f);
        await UniTask.WaitForSeconds(1f);
        _canvasGroup.alpha = 1f;


        float delayPerCharacter = sentenceTime / sentence.Length;

        for (int i = 0; i < sentence.Length; i++)
        {
            _textMeshPro.text += sentence[i];
            await UniTask.WaitForSeconds(delayPerCharacter);
        }
        
        _canvasGroup.EaseCanvasGroupAlpha(0f, afterDelayTime);
        await UniTask.WaitForSeconds(afterDelayTime);
        _canvasGroup.alpha = 0;
    }
}
