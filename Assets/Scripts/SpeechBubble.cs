using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{

    static readonly float SPEAK_DURATION = 2f;
    static readonly float TEXT_ROLL = 0.05f;
    static readonly float GARBLE_PERCENT = 0.6f;

    // -- Serialize Fields & Public --
    [SerializeField]
    private TextMeshProUGUI output;

    // -- Internal --
    private IEnumerator coroutine = null;
    private WorldObject wobj;

    private void Start()
    {
        wobj = GetComponent<WorldObject>();

        output.gameObject.SetActive(false);
    }

    private IEnumerator Speak(string text)
    {
        output.gameObject.SetActive(true);
        for (int i = 0; i <= text.Length; i++)
        {
            output.SetText(text.Substring(0, i));
            yield return new WaitForSeconds(TEXT_ROLL);
        }
        yield return new WaitForSeconds(SPEAK_DURATION);
        coroutine = null;
        output.SetText("");
        output.gameObject.SetActive(false);

        // world object updates
        Debug.Log(text);
        string garbled = GarbleText(text, GARBLE_PERCENT);
        Debug.Log(garbled);
        wobj.UpdateProxem3(text);
        wobj.UpdateProxem2(garbled);
        Debug.Log(garbled);
    }

    private string GarbleText(string text, float percent)
    {
        char[] chars = text.ToCharArray();
        for (int i = 0; i < chars.Length; i++)
        {
            if (Random.Range(0f, 1f) <= percent)
            {
                chars[i] = '#';
            }
        }
        return new string(chars);
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

    public bool IsSpeaking()
    {
        return (coroutine != null);
    }
}
