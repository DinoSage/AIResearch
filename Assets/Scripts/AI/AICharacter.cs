using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.Rendering.CameraUI;

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
    private float talkative;

    [SerializeField]
    private float thinkDelay;

    [SerializeField]
    private float convoTime;

    [SerializeField]
    private World.Text[] instructions;

    // -- Non-Serialized Fields -- 
    private List<ChatMessage> instructInfo = new List<ChatMessage>();
    private List<ChatMessage> contextInfo = new List<ChatMessage>();
    private List<ChatMessage> convoInfo = new List<ChatMessage>();

    private ChatMessage timeContext = new ChatMessage();
    private ChatMessage locationContext = new ChatMessage();
    private ChatMessage actionContext = new ChatMessage();

    private ConversationManager convoManager;
    private IEnumerator thinkCouroutine;

    private string lastSpokeTime = "not for a while";

    // -- Functions --
    void Start()
    {
        convoManager = GameObject.FindGameObjectWithTag("ChatManager").GetComponent<ConversationManager>();

        // add background message to context info
        ChatMessage background = new ChatMessage();
        background.Content = string.Format("Your name is {0}. {1}", characterName, characterBackground);
        background.Role = "system";
        contextInfo.Add(background);
        convoTime = (convoTime <= 0) ? 100f : convoTime;

        // convert instructiosn as ChatMessages
        foreach (World.Text instruction in instructions)
        {
            ChatMessage message = new ChatMessage();
            message.Content = instruction.message;
            message.Role = "system";
            instructInfo.Add(message);
        }

        // set values for permanent context messages and add to list
        timeContext.Role = "system";
        timeContext.Content = "";
        contextInfo.Add(timeContext);

        locationContext.Role = "system";
        locationContext.Content = "";
        contextInfo.Add(locationContext);

        actionContext.Role = "system";
        actionContext.Content = "";
        contextInfo.Add(actionContext);
    }

    public void Chat(string message)
    {
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = "user";
        userMessage.Content = message;
        convoInfo.Add(userMessage);

        GPTCommunicator.Prompt(Reply, World.instance.worldInfo, instructInfo, contextInfo, convoInfo);
    }

    private void Reply(ChatMessage reply)
    {
        convoInfo.Add(reply);
        convoManager.ReplaceOutput(reply.Content);
        lastSpokeTime = World.instance.GetTimeStr();
    }

    void Update()
    {
        timeContext.Content = "The time right now is " + World.instance.GetTimeStr() + ".";
    }

    public void ExitedConversation()
    {
        actionContext.Content = "You are not talking to anyone.";
        StopCoroutine(thinkCouroutine);
        thinkCouroutine = null;
    }

    public void Alert(string update)
    {
        ChatMessage worldEvent = new ChatMessage();
        string time = "This happened at time " + World.instance.GetTimeStr() + ".";
        worldEvent.Content = update + time;
        worldEvent.Role = "system";
        contextInfo.Add(worldEvent);
    }

    public void Interact()
    {
        string time = World.instance.GetTimeStr();
        actionContext.Content = "You are chatting with Ansh. You started chatting at " + time;
        
        thinkCouroutine = Thinking();
        StartCoroutine(thinkCouroutine);
        convoManager.StartConversation(this);
    }

    private void ProccessThought(ChatMessage thought)
    {
        Debug.Log(thought.Content);
        if (thought.Content.Contains("||SPEAK||"))
        {
            convoInfo.Add(thought);
            convoManager.ReplaceOutput(thought.Content);
        }
    }

    IEnumerator Thinking()
    {
        while (true)
        {
            if (!GPTCommunicator.GENERATING)
            {
                Debug.Log("Thinking now!");
                List<ChatMessage> thinkInfo = new List<ChatMessage>();
                ChatMessage thinkAction = new ChatMessage();
                thinkAction.Role = "system";
                thinkAction.Content = "WHAT DO YOU WANT TO DO? The last time someone spoke was " + lastSpokeTime + ".";
                thinkInfo.Add(thinkAction);

                GPTCommunicator.Prompt(ProccessThought, World.instance.worldInfo, instructInfo, contextInfo, convoInfo, thinkInfo);
            }

            yield return new WaitForSeconds(Mathf.Max(SAFEGUARD, thinkDelay));


            /*if (Time.time - start > Mathf.Max(1, convoTime))
            {
                ChatMessage world = new ChatMessage();
                world.Content = "You need to get back to work, say goodbye! Add the phrase \"[BYE]\" to your next response";
                world.Role = "system";
                personalInfo.Add(world);
            }

            // should NPC speak?
            float rand = UnityEngine.Random.Range(0f, 1f);
            if (rand <= talkative)
            {
                Speak();
            }*/

            /*ChatMessage probe = new ChatMessage();
            probe.Role = "system";
            probe.Content = "What do you want to do?";
            List<ChatMessage> finalInfo = instructionsInfo.Concat(contextInfo.Concat(convoInfo).ToList()).ToList();
            finalInfo.Add(probe);
            convoManager.Speak


            yield return new WaitForSeconds(Mathf.Max(SAFEGUARD, thinkDelay));*/
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        Setting setting = collision.GetComponent<Setting>();
        if (setting != null)
        {
            locationContext.Content = "You are at " + setting.settingName + ".";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Setting setting = collision.GetComponent<Setting>();
        if (setting != null)
        {
            locationContext.Content = "You don't know where you are.";
        }
    }

    IEnumerator Leave()
    {
        yield return new WaitForSeconds(3f);
        convoManager.EndConversation();
    }*/

}
