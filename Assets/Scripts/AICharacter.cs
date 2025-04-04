using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
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

    private IEnumerator thinkCouroutine;
    private SpeechBubble bubble;
    private bool temp = false;

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
        background.Content = (new ContentObject("MEMORY", string.Format("Your name is {0}. {1}", characterName, characterBackground))).ToString();
        longMem.Add(background);

        // add additional memories
        foreach (World.Text info in memories)
        {
            ContentObject temp = new ContentObject("MEMORY", info.message);

            ChatMessage message = new ChatMessage();
            message.Content = temp.ToString();
            message.Role = "system";
            longMem.Add(message);
        }
    }

    void Start()
    {
        bubble = GetComponent<SpeechBubble>();

        // add all world memories
        foreach (ChatMessage info in World.instance.worldMem)
        {
            longMem.Add(info);
        }


        // start thinking
        StartCoroutine(Thinking());
        PrintAll();
    }

    public void Chat(string message)
    {
        ContentObject temp = new ContentObject("TALK", message);
        temp.Time = World.instance.GetTimeStrAI();
        temp.Date = World.instance.GetDateStrAI();
        temp.Character = "Ansh";

        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = "user";
        userMessage.Content = temp.ToString();
        shortMem.Add(userMessage);
        Debug.Log("TEMP:" + temp);

        GPTCommunicator.Prompt(ProccessThought, longMem, shortMem);
    }

    private void ProccessThought(ChatMessage thought)
    {
        Debug.Log(thought.Content);
        ContentObject actionObj = ContentObject.FromString(thought.Content);
        actionObj.Time = World.instance.GetTimeStrAI();
        actionObj.Date = World.instance.GetDateStrAI();
        actionObj.Character = characterName;

        switch (actionObj.Category)
        {
            case "TALK":
                bubble.Display(actionObj.Message);
                break;
            case "NOTHING":
                break;
        }

        thought.Content = actionObj.ToString();
        shortMem.Add(thought);
        PrintAll();
    }

    IEnumerator Thinking()
    {
        while (true)
        {
            if (!GPTCommunicator.GENERATING)
            {
                ContentObject thinkObj = new ContentObject("THINK", "What do you want to do next?", time: World.instance.GetTimeStrAI());

                ChatMessage thinkAction = new ChatMessage();
                thinkAction.Role = "system";
                thinkAction.Content = thinkObj.ToString();
                shortMem.Add(thinkAction);

                GPTCommunicator.Prompt(ProccessThought, longMem, shortMem);
            }

            yield return new WaitForSeconds(Mathf.Max(SAFEGUARD, thinkDelay));
        }
    }

    public void Sense(string update)
    {
        ContentObject cobj = ContentObject.FromString(update);

        if (cobj.Category.Equals("TALK"))
        {
            Chat(cobj.Message);
            return;
        }

        ChatMessage message = new ChatMessage();
        message.Role = "system";
        message.Content = cobj.ToString();
        shortMem.Add(message);
    }

    void Update()
    {
        //Debug.Log(World.instance.GetDateStrAI());
        if (!temp)
        {
            ContentObject cobj = new ContentObject("EVENT", "You can now see and talk to " + characterName);
            cobj.Time = World.instance.GetTimeStrAI();
            cobj.Date = World.instance.GetDateStrAI();

            this.GetComponent<WorldObject>().UpdateProxemAll(cobj.ToString());
            temp = true;
        }
    }

    /*/// <summary>
    /// Called when someone talks to the a AI
    /// </summary>
    /// <param name="message"></param>
    public void Chat(string message)
    {
        ContentObject temp = new ContentObject("TALK", message);
        temp.Time = World.instance.GetTimeStrAI();
        temp.Date = World.instance.GetDateStrAI();
        temp.Character = "Ansh";

        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = "user";
        userMessage.Content = ContentObject.ToString(temp);
        shortMem.Add(userMessage);

        GPTCommunicator.Prompt(ProccessThought, longMem, shortMem);
    }

    public void Alert(string update)
    {
        ContentObject eventObject = new ContentObject("EVENT", update);
        eventObject.Time = World.instance.GetTimeStrAI();
        eventObject.Date = World.instance.GetDateStrAI();

        ChatMessage worldEvent = new ChatMessage();
        worldEvent.Role = "system";
        worldEvent.Content = ContentObject.ToString(eventObject);
        shortMem.Add(worldEvent);
    }

    /// <summary>
    /// Called when the player interacts with the AI
    /// </summary>
    public void Interact()
    {
        ContentObject startConvo = new ContentObject("EVENT", "You are chatting with Ansh.");
        startConvo.Time = World.instance.GetTimeStrAI();
        startConvo.Date = World.instance.GetDateStrAI();

        ChatMessage eventMessage = new ChatMessage();
        eventMessage.Role = "system";
        eventMessage.Content = ContentObject.ToString(startConvo);
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
        eventMessage.Content = ContentObject.ToString(endConvo);
        shortMem.Add(eventMessage);

        //StopCoroutine(thinkCouroutine);
        //thinkCouroutine = null;

        ContentObject summObj = new ContentObject("SUMMARIZE", "Summarize all relevant [TALK], [HI], [BYE], and [EVENT] messages. Set the [MEMORY] message's time field to reflect the latest relevant event. Mention important times within the summary where necessary. Keep it concise while preserving key details about conversations, events, and decisions.");

        ChatMessage summAction = new ChatMessage();
        summAction.Role = "system";
        summAction.Content = ContentObject.ToString(summObj);
        shortMem.Add(summAction);

        GPTCommunicator.Prompt(Summarize, longMem, shortMem);
    }

    private void Summarize(ChatMessage memory)
    {
        Debug.Log("IN SUMMARIZE!");
        ContentObject actionObj = ContentObject.FromString(memory.Content);
        actionObj.Time = null;

        memory.Content = ContentObject.ToString(actionObj);
        longMem.Add(memory);
        shortMem.Clear();
        PrintAll();
    }

    private void HIAction(ContentObject action)
    {
        convoManager.StartConversation(this);
        convoManager.ReplaceOutput(action.Message);

        ContentObject startConvo = new ContentObject("EVENT", "You are chatting with Ansh.");
        startConvo.Time = World.instance.GetTimeStrAI();
        startConvo.Date = World.instance.GetDateStrAI();

        ChatMessage eventMessage = new ChatMessage();
        eventMessage.Role = "system";
        eventMessage.Content = ContentObject.ToString(startConvo);
        shortMem.Add(eventMessage);
    }

    private void ProccessThought(ChatMessage thought)
    {
        Debug.Log("TEST: " + thought.Content);
        ContentObject actionObj = ContentObject.FromString(thought.Content);
        actionObj.Time = World.instance.GetTimeStrAI();
        actionObj.Date = World.instance.GetDateStrAI();
        actionObj.Character = characterName;
        Debug.Log("CAT: " + actionObj.Category);

        switch (actionObj.Category)
        {
            case "HI":
                convoManager.StartConversation(this);
                break;
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

        thought.Content = ContentObject.ToString(actionObj);
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
                thinkAction.Content = ContentObject.ToString(thinkObj);
                shortMem.Add(thinkAction);

                GPTCommunicator.Prompt(ProccessThought, longMem, shortMem);
            }

            yield return new WaitForSeconds(Mathf.Max(SAFEGUARD, thinkDelay));
        }
    }

    IEnumerator Leave()
    {
        while (ConversationManager.AI_SPEAKING)
        {
            yield return null;

        }
        yield return new WaitForSeconds(2f);
        convoManager.EndConversation();
    }*/


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
