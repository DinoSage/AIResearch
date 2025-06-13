using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    public static float SAFEGUARD = 3f;

    // ==============================
    //       Serialized Fields
    // ==============================

    [SerializeField]
    private float thinkDelay;

    [SerializeField]
    public World.Text[] memories;

    // ==============================
    //        Other Variables
    // ==============================

    public List<ChatMessage> longMem = new List<ChatMessage>();
    public List<ChatMessage> shortMem = new List<ChatMessage>();

    private IEnumerator thinkCouroutine;
    private SpeechBubble bubble;
    private Locator locator;
    private bool moving;


    // ==============================
    //        Unity Functions
    // ==============================

    private void Awake()
    {
        // add master instruction
        ChatMessage masterInstruction = new ChatMessage();
        masterInstruction.Role = "system";
        masterInstruction.Content = World.MASTER_INSTRUCTION;
        longMem.Add(masterInstruction);

        // add identity memory
        ChatMessage background = new ChatMessage();
        background.Role = "system";
        background.Content = (new ContentObject("MEMORY", message: $"Your name is {name}.")).ToString();
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

    private void Start()
    {
        bubble = GetComponent<SpeechBubble>();
        locator = GetComponent<Locator>();

        // add all world memories
        foreach (ChatMessage info in World.instance.worldMem)
        {
            longMem.Add(info);
        }

        // start thinking
        StartCoroutine(Thinking());
        //StartCoroutine(Move());
        //PrintAll();
    }

    // ==============================
    //       Private Functions
    // ==============================

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

    private void ProccessThought(ChatMessage thought)
    {
        Debug.Log(thought.Content);
        ContentObject actionObj = ContentObject.FromString(thought.Content);
        actionObj.Time = World.instance.GetTimeStrAI();
        actionObj.Date = World.instance.GetDateStrAI();
        actionObj.Character = name;

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

    IEnumerator Move()
    {
        yield return null;
        while (true)
        {
            Debug.Log("is locator null? " + (locator == null));
            Setting setting = locator.GetCurrSetting();
            Debug.Log("is locator setting null? " + (setting == null));
            Debug.Log("is locator rand point null? " + (locator.GetCurrSetting().RandPointInSetting()));

            Vector3 point = locator.GetCurrSetting().RandPointInSetting();
            Debug.Log("Moving to: " + point);
            StartCoroutine(Approaching(point, 0.1f));
            yield return new WaitWhile(IsMoving);
        }
        yield return null;
    }

    IEnumerator Approaching(Vector3 targetPos, float threshold)
    {
        moving = true;
        while (true)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, Time.deltaTime * 5f);
            if (Vector3.Distance(targetPos, this.transform.position) < threshold)
            {
                break;
            }
            yield return null;
        }
        moving = false;
        yield return null;
    }

    // ==============================
    //        Public Functions
    // ==============================
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

    public void Sense(string update)
    {
        ContentObject cobj = ContentObject.FromString(update);
        cobj.Time = World.instance.GetTimeStrAI();

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

    public bool IsMoving()
    {
        return moving;
    }

    public static string ConvertToString(ChatMessage message)
    {
        return $"role: {message.Role}\ncontent: {message.Content}";
    }
}
