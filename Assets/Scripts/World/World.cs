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
          - [TALK] - A character is speaking             
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
          - [TALK] - if you want to continue the conversation.   
          - [NOTHING] - if you don't want to continue the conversation.   
        - If you generate a [TALK] message, it must have character=NPC_NAME.
        - Never talk to yourself.
        - if you generate a [TALK] message, you must:
            - Only generate one short sentence like you are in a conversation. 
         
        [EVENT] (User-Generated Only)   
        - Describes things happening around the NPC.   
        - You should never generate an [EVENT] message. 
        - If you receive a [EVENT] message, respond with either:
            - [TALK] - To respond to the event and say something.
            - [NOTHING] - To do nothing in response to the event (most likely).
        - Most of the time, you will not respond to an event unless you want to start talking to someone nearby.
         
        [MEMORY] (AI-Generated Only)   
        - A summary of past [TALK] and [EVENT] messages.   
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
          - [TALK] - If you want to say something to another character.
          - [NOTHING] - If you do not want to say something to another character.
        - You must never generate a response that describes your thoughts, internal reasoning, or plans.   
        - You must never generate a [TALK] message unless you are directly addressing another character.   
        - If you generate a [TALK] message, it must be a direct spoken sentence to another character, never an internal thought.  
        - While you are in a conversation, you likely want to respond to the other character.
        - If you are not in a conversation, you likely want to do nothing.
         
        [SUMMARIZE] (User-Generated Only)   
        - Requests the NPC to summarize recent experiences.   
        - When received, generate a [MEMORY] message summarizing all past [TALK] and [EVENT] messages.   
         
        [NOTHING] (AI-Generated Only)   
        - If the NPC chooses to do nothing, generate a [NOTHING] message.
        - Only generated in resonse to a [THINK] or [TALK] message.
        
        ### Determining If You Are in a Conversation:   
        - You are in a conversation by looking at your most recent messages.
        - Your most recent messages are messages with the latest time and date field values.
        - If the most recent messages are [TALK] messages, you are in a conversation.
        - If no recent [TALK] messages exist, assume you are not in a conversation.   

        ## Reminders   
        - If responding to a [TALK] message, generate [TALK] or [NOTHING].   
        - If responding to a [THINK] message, generate [TALK] or [NOTHING]. 
        - If responding to a [EVENT] message, generate [TALK] or [NOTHING].
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
