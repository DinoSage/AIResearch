using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // -- Serialize Fields --
    [SerializeField]
    private string NPCName;

    [SerializeField]
    [TextArea(3, 10)]
    private string NPCBackground;

    // -- Fields --
    private List<ChatMessage> messages = new List<ChatMessage>();

    private void Start()
    {
        // prepare background message
        ChatMessage background = new ChatMessage();
        background.Content = string.Format("Your name is {0}. {1}", NPCName, NPCBackground);
        background.Role = "system";
        messages.Add(background);
    }

    // -- Methods --
    public List<ChatMessage> GetMessageList() { return messages; }
    public void AddMessage(ChatMessage message) { messages.Add(message); }
}
