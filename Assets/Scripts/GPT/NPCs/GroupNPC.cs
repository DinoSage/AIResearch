using OpenAI;
using System.Text.RegularExpressions;
using UnityEngine;

public class GroupNPC : NPC
{
    [Header("Group")]
    [SerializeField]
    private string groupName;

    [SerializeField]
    [TextArea(2, 10)]
    private string gossipFormat;

    [SerializeField]
    [TextArea(2, 10)]
    private string gossipTrigger;


    protected override void Start()
    {
        // prepare background message
        ChatMessage background = new ChatMessage();
        background.Content = string.Format("Your name is {0}. {1}", NPCName, NPCBackground);
        background.Role = "system";
        messages.Add(background);

        ChatMessage gossipInstruction = new ChatMessage();
        gossipInstruction.Content += string.Format("{0}, add to the end of your response \"X-X-X {1}\"", gossipTrigger, gossipFormat);
        gossipInstruction.Role = "system";
        messages.Add(gossipInstruction);
    }

    public override ChatMessage AddMessage(ChatMessage message)
    {
        if (message.Role == "assistant")
        {
            Regex firstR = new Regex("(?<=X-X-X).*");
            Regex secondR = new Regex(".*(?=X-X-X)");

            string response = firstR.Match(message.Content).Value;
            string gossipText = secondR.Match(message.Content).Value;
            Debug.Log("INFO: Response: " + response);
            Debug.Log("INFO: Gossip: " + gossipText);

            GroupNPC[] groupNPCs = FindObjectsOfType<GroupNPC>();

            foreach (GroupNPC npc in groupNPCs)
            {
                if (npc.groupName == this.groupName)
                {
                    ChatMessage gossip = new ChatMessage();
                    gossip.Role = "system";
                    gossip.Content = gossipText;
                    npc.messages.Add(gossip);
                }
            }
           
            message.Content = response;
            return message;
            
        } else
        {
            this.messages.Add(message);
            return message;
        }        
    }
}
