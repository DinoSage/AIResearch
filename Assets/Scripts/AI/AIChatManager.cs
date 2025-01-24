using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIChatManager : MonoBehaviour
{
    // -- Serialized Fields --
    [SerializeField]
    GameObject[] chatUIElements;

    [SerializeField]
    public GlobalMessage[] initialInfoText;

    // -- Non-Serialized Fields --
    private TMP_InputField input;
    private TextMeshProUGUI output;

    private OpenAIApi openAI = new OpenAIApi();

    private AICharacter currentNPC;
    private List<ChatMessage> globalInfoChat = new List<ChatMessage>();

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
        foreach (GameObject ui in chatUIElements)
        {
            ui.SetActive(false);
        }

        foreach (GlobalMessage info in initialInfoText)
        {
            ChatMessage message = new ChatMessage();
            message.Content = info.message;
            message.Role = "system";
            globalInfoChat.Add(message);
        }
    }

    public async void Prompt(Action<ChatMessage> process, params List<ChatMessage>[] lists)
    {
        IEnumerable<ChatMessage> complete = null;
        foreach (List<ChatMessage> list in lists)
        {
            complete = (complete == null) ? list.AsEnumerable() : complete.Concat(list);
        }

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Model = "gpt-4o-mini";
        request.Messages = complete.ToList();

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            process(response.Choices[0].Message);
        }
    }

    public async void Speak(List<ChatMessage> messages)
    {
        // determine final complete list of ChatMessages to generate response
        List<ChatMessage> completeList = globalInfoChat.Concat(messages).ToList();

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = completeList;
        request.Model = "gpt-4o-mini";

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            // add raesponse to history and display response in UI
            ChatMessage chatReponse = response.Choices[0].Message;
            currentNPC.AddResponse(chatReponse);

            output.SetText(chatReponse.Content);
        }
    }

    public void StartConversation(AICharacter npc)
    {
        // show conversation UI elements so player can converse
        foreach (GameObject ui in chatUIElements)
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
        foreach (GameObject ui in chatUIElements)
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

    public void OnLeave()
    {
        // end conversation if leaving conversation input received
        if (currentNPC != null)
        {
            EndConversation();
        }
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

            ChatMessage message = new ChatMessage();
            message.Content = input.text;
            message.Role = "user";

            currentNPC.Chat(message);
            input.text = "";
        }
    }
}
