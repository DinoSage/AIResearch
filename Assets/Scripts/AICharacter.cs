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

    List<ChatMessage> longMem = new List<ChatMessage>();
    List<ChatMessage> shortMem = new List<ChatMessage>();

    private IEnumerator thinkCouroutine;
    private SpeechBubble bubble;


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

        // add all world memories
        foreach (ChatMessage info in World.instance.worldMem)
        {
            longMem.Add(info);
        }

        // start thinking
        StartCoroutine(Thinking());
        StartCoroutine(Move());
        PrintAll();
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
        int nonDoorCount = 0;
        while (true)
        {
            Locator locator = GetComponent<Locator>();
            int randInt = (nonDoorCount <= 3) ? UnityEngine.Random.Range(1, 4) : 0;
            WorldObject[] targets = null;
            if (randInt == 0)
            {
                targets = locator.GetCurrSetting().GetDoors();
                nonDoorCount = 0;
            }
            else if (randInt == 1)
            {
                targets = locator.GetCurrSetting().GetItems();
                nonDoorCount++;
            }
            else if (randInt == 2)
            {
                targets = locator.GetCurrSetting().GetCharacters();
                nonDoorCount++;
            }
            else
            {
                Debug.LogError("Should not be reaching here!");
            }

            WorldObject randTarget = targets[UnityEngine.Random.Range(0, targets.Length)];

            StartCoroutine(Approach(randTarget));


            yield return null;
        }

    }

    IEnumerator Approach(WorldObject obj)
    {
        bool approaching = true;

        Vector3 targetPos = obj.transform.position;

        while (approaching)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, obj.transform.position, Time.deltaTime * 5f);
            if (Vector3.Distance(targetPos, this.transform.position) < 0.01f)
            {
                approaching = false;
            }
            yield return null;
        }
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
}
