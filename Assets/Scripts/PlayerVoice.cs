using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVoice : MonoBehaviour
{
    // -- Serialize Fields & Public --
    [SerializeField]
    private TMP_InputField textInput;

    // -- Internal --
    private PlayerInput input;
    private SpeechBubble bubble;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        bubble = GetComponent<SpeechBubble>();

        OnPlayerMode();
    }

    void Update()
    {

    }

    // -- External --

    public void OnPlayerMode()
    {
        textInput.gameObject.SetActive(false);
        input.SwitchCurrentActionMap("Player");
    }

    public void OnConvoMode()
    {
        textInput.gameObject.SetActive(true);
        input.SwitchCurrentActionMap("Conversation");
    }

    public void OnEnter()
    {
        if (!bubble.IsSpeaking())
        {
            bubble.Display(textInput.text);
            textInput.text = "";
        }
    }

}
