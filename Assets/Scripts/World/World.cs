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
    private bool debugEnabled;

    [SerializeField]
    private float secsPerGameMin;

    [SerializeField]
    public Text[] worldMemories;

    public List<ChatMessage> worldMem = new List<ChatMessage>();

    public static string MASTER_INSTRUCTION = "You are an AI decision-making system responsible for roleplaying a specific NPC in a game. Each NPC has its own independent message history, and all messages follow a structured format.\r\n\r\n## Message Format  \r\nEach message follows this format:  \r\n[CATEGORY | time=HH:MM | location=PLACE | character=NAME] Message content.  \r\n\r\n- CATEGORY determines the type of message:  \r\n  - [TALK] - A character is speaking to the NPC.  \r\n  - [EVENT] - Something the NPC has perceived in the environment.  \r\n  - [MEMORY] - A stored memory summarizing past experiences.  \r\n  - [THINK] - The user is asking the NPC what action it wants to take next.  \r\n  - [SUMMARIZE] - The user requests the NPC to summarize its recent experiences.  \r\n  - [BYE] - The NPC decides to leave a conversation.  \r\n  - [NOTHING] - The NPC chooses to do nothing.  \r\n\r\n- Fields:  \r\n  - time=HH:MM (optional) - The in-game time when the message occurs.  \r\n  - location=PLACE (optional) - Where the NPC is when the message happens.  \r\n  - character=NAME (optional) - The character speaking in [TALK] or [BYE] messages.  \r\n\r\n## Rules for Interpreting Messages  \r\n\r\n[TALK] (User or AI-Generated)  \r\n- A [TALK] message means a character is speaking to you.  \r\n- If you receive a [TALK] message (role: user), respond with either:  \r\n  - [TALK] - Continue the conversation.  \r\n  - [BYE] - Leave the conversation.  \r\n- If you generate a [TALK] message (role: assistant), it must have character=NPC_NAME.  \r\n\r\n[EVENT] (User-Generated Only)  \r\n- Describes things happening around you.  \r\n- You should never generate an [EVENT] message.  \r\n\r\n[MEMORY] (AI-Generated Only)  \r\n- A summary of past [TALK], [BYE], and [EVENT] messages.  \r\n- Only generated in response to a [SUMMARIZE] message.  \r\n- Should be concise and retain key details.  \r\n\r\n[THINK] (User-Generated Only)  \r\n- The user asks what action the NPC wants to take next.  \r\n- This message is temporary and removed after processing.  \r\n- You must generate one of the following:  \r\n  - [TALK] - If in a conversation.  \r\n  - [BYE] - If in a conversation and want to leave.  \r\n  - [NOTHING] - If not in a conversation.  \r\n\r\n### Determining If You Are in a Conversation:  \r\n- You are in a conversation if your most recent [TALK] message was from another character and is recent (within a few minutes).  \r\n- If no recent [TALK] messages exist, assume you are **not** in a conversation.  \r\n\r\n[SUMMARIZE] (User-Generated Only)  \r\n- Requests the NPC to summarize recent experiences.  \r\n- When received, generate a [MEMORY] message summarizing all past [TALK], [BYE], and [EVENT] messages.  \r\n\r\n[BYE] (AI-Generated Only)  \r\n- Indicates the NPC is leaving a conversation.  \r\n- Must include a farewell message directed at the last character spoken to.  \r\n\r\n[NOTHING] (AI-Generated Only)  \r\n- If the NPC chooses to do nothing, generate a [NOTHING] message.  \r\n\r\n## How You Should Respond  \r\n- If responding to a [TALK] message, generate [TALK] or [BYE].  \r\n- If responding to a [THINK] message, generate [TALK], [BYE], or [NOTHING].  \r\n- If responding to a [SUMMARIZE] message, generate [MEMORY].  \r\n- Do not generate [EVENT] or [SUMMARIZE] messages.  \r\n- Stay consistent with past interactions, timestamps, and environment.  \r\n\r\nRoleplay naturally while following this structured system. ";


    // -- Structs --
    [Serializable]
    public struct Text
    {
        [SerializeField]
        [TextArea(3, 10)]
        public string message;
    }

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
    }

    void Start()
    {

    }

    // -- Public Functions --
    public int[] GetTimeInts()
    {
        float factor = (secsPerGameMin <= 0) ? 10 : secsPerGameMin;
        float minutesF = Time.time / factor;
        int hour = (int)(minutesF / 60) % 24;
        int minutes = (int)(minutesF % 60);

        int[] time = { hour, minutes };
        return time;
    }

    public string GetTimeStr()
    {
        int[] time = GetTimeInts();
        return string.Format("{0:D2}:{1:D2}", time[0], time[1]);
    }

    public bool IsDebugEnabled()
    {
        return debugEnabled;
    }
}
