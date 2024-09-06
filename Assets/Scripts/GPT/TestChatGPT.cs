using OpenAI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestChatGPT : MonoBehaviour
{
    [SerializeField]
    TMP_InputField input;

    [SerializeField]
    TextMeshProUGUI output;

    private OpenAIApi openAI = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();

    public async void AskChatGPT(string newText)
    {
        ChatMessage message = new ChatMessage();
        message.Content = newText;
        message.Role = "user";
        messages.Add(message);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-4o-mini";

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatReponse = response.Choices[0].Message;
            messages.Add(chatReponse);

            Debug.Log(chatReponse.Content);
            input.text = "";
            output.SetText(chatReponse.Content);
        }
    }

}
