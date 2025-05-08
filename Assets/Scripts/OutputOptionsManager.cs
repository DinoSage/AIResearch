using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OutputOptionsManager : MonoBehaviour
{
    public static readonly float CHARACTERS_PER_SECOND = 7f;

    [SerializeField] GameObject outputPanel;
    [SerializeField] TextMeshProUGUI outputText;
    [SerializeField] Button[] outputButtons;

    private void Start()
    {
        DisableOutput();
    }

    private void DisableOutput()
    {
        outputPanel.SetActive(false);
    }

    IEnumerator DelayedDisable(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        DisableOutput();
    }

    public void SetOutputDetails(string text, string[] names, UnityAction[] actions)
    {
        outputPanel.SetActive(true);
        outputText.text = text;

        for (int i = 0; i < outputButtons.Length; i++)
        {
            Button button = outputButtons[i];
            button.onClick.RemoveAllListeners();

            UnityAction action = (i < actions.Length) ? actions[i] : null;
            string name = (names != null && i < names.Length) ? names[i] : $"DEFAULT {i}";

            if (action != null)
            {
                button.gameObject.SetActive(true);
                button.onClick.AddListener(action);
                button.onClick.AddListener(DisableOutput);
                button.GetComponentInChildren<TextMeshProUGUI>().text = name;
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    public void SetOutputDetails(string text)
    {
        outputPanel.SetActive(true);
        outputText.text = text;

        foreach (var button in outputButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }

        int seconds = 1 + (int)(text.Length / CHARACTERS_PER_SECOND);
        StartCoroutine(DelayedDisable(seconds));
    }
}
