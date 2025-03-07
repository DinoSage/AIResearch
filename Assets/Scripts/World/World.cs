using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConversationManager;

public class World : MonoBehaviour
{
    // -- Singleton Setup --
    public static World instance;

    // -- Serialize Fields --
    [SerializeField]
    private string startDate;

    [SerializeField]
    private string startTime;

    [SerializeField]
    private bool debugEnabled;

    [SerializeField]
    private float secsPerGameMin;

    [SerializeField]
    public Text[] worldMemories;

    public List<ChatMessage> worldMem = new List<ChatMessage>();

    public static string MASTER_INSTRUCTION = @"You are an AI decision-making system responsible for roleplaying a specific NPC in a game.  
        Each NPC has its own independent message history, and all messages follow a structured format. 
         
        ## Message Format   
        Each message follows this format:   
        [CATEGORY | date=YYYY-MM-DD | time=HH:MM | location=PLACE | character=NAME] Message content.   
         
        - CATEGORY determines the type of message:
          - [TALK] - A character is speaking to the NPC or the NPC is speaking to the character.             
          - [HI] - The NPC decides to start a conversation
          - [BYE] - The NPC decides to leave a conversation.
          - [MEMORY] - A stored memory summarizing past experiences.   
          - [EVENT] - Something the NPC has perceived in the environment.   
          - [THINK] - The user requests the NPC what action it wants to take next.   
          - [SUMMARIZE] - The user requests the NPC to summarize its recent experiences. 
          - [NOTHING] - The NPC chooses to do nothing.   
         
        Each field provides context for the message that follows. 
        By default for each message category, do not provide values for these fields unless asked as you will see later.
        - Fields:   
          - date=YYYY-MM-DD (optional) - The in-game date when the message occurs.
          - time=HH:MM (optional) - The in-game time when the message occurs.   
          - location=PLACE (optional) - Where the NPC is when the message happens.   
          - character=NAME (optional) - The character speaking in [TALK] or [BYE] messages.   
         
        ## Rules for Interpreting and Responding to Messages   
         
        [TALK] (User or AI-Generated)   
        - A response from the NPC or the character the NPC is speaking to.   
        - If you receive a [TALK] message, respond with either:   
          - [TALK] - Continue the conversation.   
          - [BYE] - Leave the conversation.   
        - If you generate a [TALK] message, it must have character=NPC_NAME.   

        [HI] (AI-Generated Only)
        - Indicates the NPC is starting a conversation.
        - You will never receive a [HI] message.
        - If you generate a [HI] message, you must:
            - Must include a hello message directed to the character you want to speak to.
            - Must have field character=NAME be the name of the character you want to speak to.

        [BYE] (AI-Generated Only)   
        - Indicates the NPC is leaving a conversation.   
        - If you generate a [BYE] message, you must:
            - Must include a farewell message directed at the last character spoken to.   
         
        [EVENT] (User-Generated Only)   
        - Describes things happening around the NPC.   
        - You should never generate an [EVENT] message.   
         
        [MEMORY] (AI-Generated Only)   
        - A summary of past [TALK], [HI], [BYE], and [EVENT] messages.   
        - Only generated in response to a [SUMMARIZE] message.   
        - Should be concise and retain key details.
        - If you generate a [MEMORY] message, you must:
            - Be concise and retain key details.
            - Must include the time of important details.
            - Must include the date or dates of important details.
         
        [THINK] (User-Generated Only)   
        - The user asks what action the NPC wants to take next.   
        - This message is temporary and removed after processing.   
        - If you receive a [THINK] message, respond with either:   
          - [TALK] - If in a conversation.   
          - [BYE] - If in a conversation and want to leave.
          - [HI] - If not in a conversation and want to start one.
          - [NOTHING] - If not in a conversation.
         
        [SUMMARIZE] (User-Generated Only)   
        - Requests the NPC to summarize recent experiences.   
        - When received, generate a [MEMORY] message summarizing all past [TALK], [HI], [BYE], and [EVENT] messages.   
         
        [NOTHING] (AI-Generated Only)   
        - If the NPC chooses to do nothing, generate a [NOTHING] message.
        - Only generated in resonse to a [THINK] message.
        
        ### Determining If You Are in a Conversation:   
        - You are in a conversation if your most recent [TALK] message was from another character and is recent (within a few minutes).   
        - If no recent [TALK] messages exist, assume you are **not** in a conversation.   

        ## Reminders   
        - If responding to a [TALK] message, generate [TALK] or [BYE].   
        - If responding to a [THINK] message, generate [TALK], [HI], [BYE], or [NOTHING].   
        - If responding to a [SUMMARIZE] message, generate [MEMORY].   
        - Do not generate [EVENT] or [SUMMARIZE] messages.   
        - Stay consistent with past interactions, timestamps, and environment.   
         
        Roleplay naturally while following this structured system.";



    // -- Structs --
    [Serializable]
    public struct Text
    {
        [SerializeField]
        [TextArea(3, 10)]
        public string message;
    }

    // -- Private Fields --
    private DateTime dateTime;

    void Awake()
    {
        instance = this;
        // convert serialized world background into list of ChatMessages
        foreach (Text info in worldMemories)
        {
            ContentObject temp = new ContentObject("MEMORY", info.message);

            ChatMessage message = new ChatMessage();
            message.Content = ContentObject.ObjectToString(temp);
            message.Role = "system";
            worldMem.Add(message);
        }
        dateTime = DateTime.Parse(startDate + " " + startTime);
        StartCoroutine(WorldTick());
    }

    void Start()
    {

    }

    // -- Public Functions --

    public string GetTimeStrClock()
    {
        //return string.Format("{0:D2}:{1:D2}", dateTime.Hour, dateTime.Minute);
        return dateTime.ToShortTimeString();
    }

    public string GetDateStrCalendar()
    {
        //return dateTime.ToString("D");
        return dateTime.ToString("MMM d, yyyy");
    }

    public string GetTimeStrAI()
    {
        return dateTime.ToString("HH:mm");
    }

    public string GetDateStrAI()
    {
        return dateTime.ToString("yyyy-MM-dd");
    }

    public bool IsDebugEnabled()
    {
        return debugEnabled;
    }

    IEnumerator WorldTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(secsPerGameMin);
            dateTime = dateTime.AddMinutes(1);
        }
    }
}
