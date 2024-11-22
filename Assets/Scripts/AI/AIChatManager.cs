using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AIChatManager : MonoBehaviour
{
    // -- Serialized Fields --
    [SerializeField]
    GameObject[] uiElements;

    [SerializeField]
    public GlobalMessage[] initialInfo;

    
    // -- Non-Serialized Fields --
    private TMP_InputField input;
    private TextMeshProUGUI output;
    private OpenAIApi openAI = new OpenAIApi();
    private AICharacter currentNPC;
    private List<ChatMessage> globalInfo = new List<ChatMessage>();

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
        foreach (GameObject ui in uiElements)
        {
            ui.SetActive(false);
        }

        foreach (GlobalMessage info in initialInfo)
        {
            ChatMessage message = new ChatMessage();
            message.Content = info.message;
            message.Role = "system";
            globalInfo.Add(message);
        }
    }

    public async void Speak()
    {
        AICharacter aiNPC = (AICharacter)currentNPC;

        List<ChatMessage> completeList = globalInfo.Concat(aiNPC.personalInfo).ToList();

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = completeList;
        request.Model = "gpt-4o-mini";

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            ChatMessage chatReponse = response.Choices[0].Message;
            aiNPC.personalInfo.Add(chatReponse);

            output.SetText(chatReponse.Content);
        }
    }

    public async void Respond()
    {
        if (input.text.Length < 1)
        {
            Debug.LogWarning("GPT: input is practically empty");
            return;
        }

        AICharacter aiNPC = (AICharacter) currentNPC; 

        ChatMessage message = new ChatMessage();
        message.Content = input.text;
        message.Role = "user";
        aiNPC.personalInfo.Add(message);

        List<ChatMessage> completeList = globalInfo.Concat(aiNPC.personalInfo).ToList();

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = completeList;
        request.Model = "gpt-4o-mini";

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            ChatMessage chatReponse = response.Choices[0].Message;
            aiNPC.personalInfo.Add(chatReponse);

            input.text = "";
            output.SetText(chatReponse.Content);
        }
    }

    public void StartConversation(AICharacter npc)
    {
        // show conversation UI elements so player can converse
        foreach (GameObject ui in uiElements)
        {
            ui.SetActive(true);
        }
        currentNPC = npc;
        currentNPC.ConversationStarted();

        // disable player movement while in conversation
        Player.instance.GetComponent<PlayerMovement>().DisableMovement();
    }

    public void EndConversation()
    {
        // hide conversation UI elements since no longer in conversation
        foreach (GameObject ui in uiElements)
        {
            ui.SetActive(false);
        }

        // clear converation UI text
        input.text = "";
        output.SetText("");

        currentNPC.ConversationEnded();
        currentNPC = null;

        // enable player movement once conversation ended
        Player.instance.GetComponent<PlayerMovement>().EnableMovement();
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
            Respond();
        }
    }
}
