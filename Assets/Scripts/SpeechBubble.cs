using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{

    static readonly float SPEAK_DURATION = 3f;
    static readonly float TEXT_ROLL = 0.05f;


    // -- Serialize Fields & Public --
    [SerializeField]
    private TextMeshProUGUI output;

    // -- Internal --
    private IEnumerator coroutine = null;

    private IEnumerator Speak(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            output.SetText(text.Substring(0, i));
            yield return new WaitForSeconds(TEXT_ROLL);
        }
        yield return new WaitForSeconds(SPEAK_DURATION);
        coroutine = null;
    }

    // -- External --
    public void Display(string text)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = Speak(text);
        StartCoroutine(coroutine);
    }
}
