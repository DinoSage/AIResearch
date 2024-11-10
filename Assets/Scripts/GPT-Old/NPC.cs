using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    // -- Fields --
    [SerializeField]
    protected string NPCName;

    [SerializeField]
    [TextArea(3, 10)]
    protected string NPCBackground;
    

    protected List<ChatMessage> messages = new List<ChatMessage>();
    
    
    // -- Methods --
    protected abstract void Start();

    public abstract ChatMessage AddMessage(ChatMessage message);

    public List<ChatMessage> GetMessageList() { return messages; }


}
