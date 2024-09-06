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

    // -- Methods --
    public List<ChatMessage> GetMessageList() { return messages; }
    public void AddMessage(ChatMessage message) { messages.Add(message); }
}
