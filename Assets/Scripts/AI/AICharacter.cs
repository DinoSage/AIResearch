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
    [NonSerialized] public bool responding = false;

    private List<ChatMessage> instructInfo = new List<ChatMessage>();
    private List<ChatMessage> contextInfo = new List<ChatMessage>();
    private List<ChatMessage> convoInfo = new List<ChatMessage>();

    private ChatMessage timeContext = new ChatMessage();
    private ChatMessage locationContext = new ChatMessage();
    private ChatMessage actionContext = new ChatMessage();

    private ConversationManager convoManager;
    private IEnumerator coroutine;

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

        // set roles for some context messages
        timeContext.Role = "system";
        locationContext.Role = "system";
        actionContext.Role = "system";
    }

    public void Chat(string message)
    {
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = "user";
        userMessage.Content = message;
        convoInfo.Add(userMessage);

        GPTCommunicator.Prompt(Reply, World.instance.worldInfo, contextInfo, convoInfo);
    }

    private void Reply(ChatMessage reply)
    {
        convoInfo.Add(reply);
        convoManager.ReplaceOutput(reply.Content);
    }

    void Update()
    {
        Debug.Log(GPTCommunicator.GENERATING);
        timeContext.Content = "The time is " + World.instance.GetTimeStr() + ".";
    }

    public void ExitedConversation()
    {
        actionContext.Content = "You are not talking to anyone.";
        StopCoroutine(coroutine);
        coroutine = null;
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
        /*string time = World.instance.GetTimeStr();
        actionContext.Content = "You are now talking to Ansh";
        actionContext.Content += "The time right now is " + time;
        
        coroutine = Thinking();
        StartCoroutine(coroutine);
        convoManager.StartConversation(this);
        responding = false;
        Debug.Log("Entered Conversation");*/
        /*List<ChatMessage> test = new List<ChatMessage>();
        ChatMessage testMesage = new ChatMessage();
        testMesage.Role = "user";
        testMesage.Content = "hi! how is the weather today?";
        test.Add(testMesage);
        GPTCommunicator.Prompt(Blah, test);*/
        convoManager.StartConversation(this);
    }

    public void Speak()
    {
        if (!responding) {
            responding = true;
            List<ChatMessage> finalInfo = contextInfo.Concat(convoInfo).ToList();
            //convoManager.Speak(finalInfo);
        }
    }

    public void AddResponse(ChatMessage response)
    {
        convoInfo.Add(response);
        if (response.Content.Contains("[BYE]")) {
            StartCoroutine(Leave());
        }
        responding = false;
    }

    IEnumerator Thinking()
    {
        float start = Time.time;
        while (true)
        {
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

    private void OnTriggerEnter2D(Collider2D collision)
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
    }

}
