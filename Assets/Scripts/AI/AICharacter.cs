using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour, IInteractable
{
    public static float SAFEGUARD = 3f;

    // -- Serialize Fields --

    [Header("Background")]
    [SerializeField]
    private string characterName;

    [SerializeField]
    [TextArea(3, 10)]
    private string characterBackground;

    [SerializeField]
    private float thinkDelay;

    [SerializeField]
    public World.Text[] memories;


    // -- Non-Serialized Fields -- 
    List<ChatMessage> longMem = new List<ChatMessage>();
    List<ChatMessage> shortMem = new List<ChatMessage>();

    private ConversationManager convoManager;
    private IEnumerator thinkCouroutine;

    // -- Functions --
    void Awake()
    {
        // add master instruction
        ChatMessage masterInstruction = new ChatMessage();
        masterInstruction.Role = "system";
        masterInstruction.Content = World.MASTER_INSTRUCTION;
        longMem.Add(masterInstruction);

        // add identity memory
        ChatMessage background = new ChatMessage();
        background.Role = "system";
        background.Content = ContentObject.ObjectToString(new ContentObject("MEMORY", string.Format("Your name is {0}. {1}", characterName, characterBackground)));
        longMem.Add(background);

        foreach (World.Text info in memories)
        {
            ContentObject temp = new ContentObject("MEMORY", info.message);

            ChatMessage message = new ChatMessage();
            message.Content = ContentObject.ObjectToString(temp);
            message.Role = "system";
            longMem.Add(message);
        }

        /*foreach (ChatMessage prompt in promptMem)
        {
            shortMem.Remove(prompt);
        }
        promptMem.Clear();*/
    }

    void Start()
    {
        convoManager = GameObject.FindGameObjectWithTag("ChatManager").GetComponent<ConversationManager>();
        foreach (ChatMessage info in World.instance.worldMem)
        {
            longMem.Add(info);
        }
        PrintAll();
    }

    void Update()
    {
        //Debug.Log(World.instance.GetDateStrAI());
    }

    public void Chat(string message)
    {
        ContentObject temp = new ContentObject("TALK", message);
        temp.Time = World.instance.GetTimeStrAI();
        temp.Date = World.instance.GetDateStrAI();
        temp.Character = "Ansh";

        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = "user";
        userMessage.Content = ContentObject.ObjectToString(temp);
        shortMem.Add(userMessage);

        GPTCommunicator.Prompt(ProccessThought, longMem, shortMem);
    }

    /*public void Alert(string update)
    {
        ChatMessage worldEvent = new ChatMessage();
        string time = "This happened at time " + World.instance.GetTimeStrClock() + ".";
        worldEvent.Content = update + time;
        worldEvent.Role = "system";
    }*/

    public void Interact()
    {
        ContentObject startConvo = new ContentObject("EVENT", "You are chatting with Ansh.");
        startConvo.Time = World.instance.GetTimeStrAI();
        startConvo.Date = World.instance.GetDateStrAI();

        ChatMessage eventMessage = new ChatMessage();
        eventMessage.Role = "system";
        eventMessage.Content = ContentObject.ObjectToString(startConvo);
        shortMem.Add(eventMessage);

        thinkCouroutine = Thinking();
        StartCoroutine(thinkCouroutine);
        convoManager.StartConversation(this);
        PrintAll();
    }
    public void ExitedConversation()
    {
        ContentObject endConvo = new ContentObject("EVENT", "You are no longer chatting with Ansh.");
        endConvo.Time = World.instance.GetTimeStrAI();
        endConvo.Date = World.instance.GetDateStrAI();

        ChatMessage eventMessage = new ChatMessage();
        eventMessage.Role = "system";
        eventMessage.Content = ContentObject.ObjectToString(endConvo);
        shortMem.Add(eventMessage);

        StopCoroutine(thinkCouroutine);
        thinkCouroutine = null;

        ContentObject summObj = new ContentObject("SUMMARIZE", "Summarize all relevant [TALK], [BYE], and [EVENT] messages. Set the [MEMORY] message's time field to reflect the latest relevant event. Mention important times within the summary where necessary. Keep it concise while preserving key details about conversations, events, and decisions.");

        ChatMessage summAction = new ChatMessage();
        summAction.Role = "system";
        summAction.Content = ContentObject.ObjectToString(summObj);
        shortMem.Add(summAction);

        GPTCommunicator.Prompt(Summarize, longMem, shortMem);
    }

    private void Summarize(ChatMessage memory)
    {
        ContentObject actionObj = ContentObject.StringToObject(memory.Content);
        actionObj.Time = null;

        memory.Content = ContentObject.ObjectToString(actionObj);
        longMem.Add(memory);
        shortMem.Clear();
        PrintAll();
    }

    private void ProccessThought(ChatMessage thought)
    {
        Debug.Log("TEST: " + thought.Content);
        ContentObject actionObj = ContentObject.StringToObject(thought.Content);
        actionObj.Time = World.instance.GetTimeStrAI();
        actionObj.Date = World.instance.GetDateStrAI();
        actionObj.Character = characterName;
        Debug.Log(actionObj.Category);

        switch (actionObj.Category)
        {
            case "TALK":
                convoManager.ReplaceOutput(actionObj.Message);
                break;
            case "BYE":
                convoManager.ReplaceOutput(actionObj.Message);
                StartCoroutine(Leave());
                break;
            case "NOTHING":
                break;
        }

        thought.Content = ContentObject.ObjectToString(actionObj);
        shortMem.Add(thought);
        PrintAll();
    }

    IEnumerator Thinking()
    {
        while (true)
        {
            if (!GPTCommunicator.GENERATING && !ConversationManager.AI_SPEAKING)
            {
                ContentObject thinkObj = new ContentObject("THINK", "What do you want to do next?", time: World.instance.GetTimeStrAI());

                ChatMessage thinkAction = new ChatMessage();
                thinkAction.Role = "system";
                thinkAction.Content = ContentObject.ObjectToString(thinkObj);
                shortMem.Add(thinkAction);

                GPTCommunicator.Prompt(ProccessThought, longMem, shortMem);
            }

            yield return new WaitForSeconds(Mathf.Max(SAFEGUARD, thinkDelay));
        }
    }

    IEnumerator Leave()
    {
        yield return new WaitForSeconds(3f);
        convoManager.EndConversation();
    }


    private void PrintAll()
    {
        foreach (ChatMessage message in longMem)
        {
            Debug.Log(String.Format("role: {0} \t content: {1}", message.Role, message.Content));
        }
        foreach (ChatMessage message in shortMem)
        {
            Debug.Log(String.Format("role: {0} \t content: {1}", message.Role, message.Content));
        }
        Debug.Log("");
        Debug.Log("");

    }
}
