using OpenAI;
using System;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class GroupNPC : NPC
{
    [Header("Group")]
    [SerializeField]
    private string groupName;

    [SerializeField]
    [TextArea(2, 10)]
    private string gossipTrigger;

    [SerializeField]
    [TextArea(2, 10)]
    private string gossipFormat;


    protected override void Start()
    {
        // prepare background message
        ChatMessage background = new ChatMessage();
        background.Content = string.Format("Your name is {0}. {1}", NPCName, NPCBackground);
        background.Role = "system";
        messages.Add(background);

        ChatMessage gossipInstruction = new ChatMessage();
        gossipInstruction.Content += string.Format("{0}, add to the very end of your response on a new line \"[ {1} ]\"", gossipTrigger, gossipFormat);
        gossipInstruction.Role = "system";
        messages.Add(gossipInstruction);
    }

    public override ChatMessage AddMessage(ChatMessage message)
    {
        if (message.Role == "assistant")
        {
            Debug.Log("INFO: " + message.Content);

            Regex r = new Regex("(?<=\\[)[^\\]]*(?=\\])");

            Match m = r.Match(message.Content);
            if (m.Success)
            {
                string gossipText = m.Value;
                string response = message.Content.Replace("[" + gossipText + "]", string.Empty);

                Debug.Log("SPE: Response: " + response);
                Debug.Log("SPE: Gossip: " + gossipText);

                GroupNPC[] groupNPCs = FindObjectsOfType<GroupNPC>();

                foreach (GroupNPC npc in groupNPCs)
                {
                    if (npc.groupName == this.groupName)
                    {
                        Debug.Log("MATCH: Name is " + npc.name);
                        ChatMessage gossip = new ChatMessage();
                        gossip.Role = "system";
                        gossip.Content = gossipText;
                        npc.messages.Add(gossip);
                    }
                }

                message.Content = response;
            }
            this.messages.Add(message);
            return message;
            
        }

        this.messages.Add(message);
        return message;

    }
}
