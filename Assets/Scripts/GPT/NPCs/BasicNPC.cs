using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicNPC : NPC
{

    protected override void Start()
    {
        // prepare background message
        ChatMessage background = new ChatMessage();
        background.Content = string.Format("Your name is {0}. {1}", NPCName, NPCBackground);
        background.Role = "system";
        messages.Add(background);
    }

    public override ChatMessage AddMessage(ChatMessage message)
    {
        this.messages.Add(message);
        return message;
    }
}
