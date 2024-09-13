using OpenAI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    // -- Serialize Fields --
    [SerializeField]
    GameObject chatUI;

    [SerializeField]
    TMP_InputField input;

    [SerializeField]
    TextMeshProUGUI output;

    // -- Fields --
    private OpenAIApi openAI = new OpenAIApi();
    private NPC currentNPC;

    public async void ChatNPC()
    {
        if (currentNPC == null)
        {
            Debug.LogWarning("GPT: current BasicNPC is null");
            return;
        }

        if (input.text.Length < 1)
        {
            Debug.LogWarning("GPT: input is practically empty");
            return;
        }

        ChatMessage message = new ChatMessage();
        message.Content = input.text;
        message.Role = "user";
        currentNPC.AddMessage(message);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = currentNPC.GetMessageList();
        request.Model = "gpt-4o-mini";

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            ChatMessage chatReponse = response.Choices[0].Message;
            ChatMessage actualResponse = currentNPC.AddMessage(chatReponse);

            Debug.Log(actualResponse.Content);
            input.text = "";
            output.SetText(actualResponse.Content);
        }
    }

    // -- Methods --
    public void StartConversation(GameObject npc)
    {
        chatUI.SetActive(true);
        currentNPC = npc.GetComponent<NPC>();
    }

    public void EndConversation()
    {
        chatUI.SetActive(false);
        currentNPC = null;
    }

}
