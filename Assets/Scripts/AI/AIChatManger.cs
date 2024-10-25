using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AIChatManger : MonoBehaviour, IChat
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
    private ICharacter currentNPC;
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

    public async void Chat()
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

    public void StartConversation(ICharacter npc)
    {
        foreach (GameObject ui in uiElements)
        {
            ui.SetActive(true);
        }
        //npc.ConversationStarted();
        currentNPC = npc;
    }

    public void EndConversation()
    {
        foreach (GameObject ui in uiElements)
        {
            ui.SetActive(false);
        }
        //currentNPC.ConversationEnded();
        currentNPC = null;
    }
}
