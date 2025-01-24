using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConversationManager : MonoBehaviour
{
    // -- Serialized Fields --
    [SerializeField]
    GameObject[] convoUI;

    [SerializeField]
    public GlobalMessage[] initialInfoText;

    // -- Non-Serialized Fields --
    private TMP_InputField input;
    private TextMeshProUGUI output;

    private AICharacter currentNPC;

    // -- Structs --
    [Serializable]
    public struct GlobalMessage
    {
        [SerializeField]
        [TextArea(3, 10)]
        public string message;
    }


    // -- Functions --
    private void Start()
    {
        this.input = GameObject.FindGameObjectWithTag("ChatInput").GetComponent<TMP_InputField>();
        this.output = GameObject.FindGameObjectWithTag("ChatOutput").GetComponent<TextMeshProUGUI>();
        
        foreach (GameObject ui in convoUI)
        {
            ui.SetActive(false);
        }
    }

    public void ClearInput(string outputText)
    {
        input.text = "";
    }

    public void ReplaceOutput(string outputText)
    {
        output.SetText(outputText);
    }

    public void StartConversation(AICharacter npc)
    {
        // show conversation UI elements so player can converse
        foreach (GameObject ui in convoUI)
        {
            ui.SetActive(true);
        }
        currentNPC = npc;

        // disable player movement while in conversation
        Player.instance.GetComponent<PlayerInput>().enabled = false;
    }

    public void EndConversation()
    {
        // hide conversation UI elements since no longer in conversation
        foreach (GameObject ui in convoUI)
        {
            ui.SetActive(false);
        }

        // clear converation UI text
        input.text = "";
        output.SetText("");

        currentNPC.ExitedConversation();
        currentNPC = null;

        // enable player movement once conversation ended
        Player.instance.GetComponent<PlayerInput>().enabled = true;
    }

    public void OnTalk()
    {
        // chat with npc if in conversation
        if (currentNPC != null)
        {
            if (input.text.Length < 1)
            {
                Debug.LogWarning("GPT: input is practically empty");
                return;
            }

            Debug.Log(input.text);
            currentNPC.Chat(input.text);
            input.text = "";
        }
    }
    public void OnLeave()
    {
        // end conversation if leaving conversation input received
        if (currentNPC != null)
        {
            EndConversation();
        }
    }
}
