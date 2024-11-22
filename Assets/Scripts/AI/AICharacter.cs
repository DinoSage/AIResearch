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
    private string CharacterName;

    [SerializeField]
    [TextArea(3, 10)]
    private string CharacterBackground;

    [SerializeField]
    private float talkative;

    [SerializeField]
    private float thinkDelay;

    // -- Non-Serialized Fields -- 
    [NonSerialized] public bool responding = false;

    [NonSerialized] public List<ChatMessage> personalInfo = new List<ChatMessage>();
    private bool inConversation = false;
    private AIChatManager chatManager;
    private IEnumerator coroutine;

    // -- Functions --
    void Start()
    {
        chatManager = GameObject.FindGameObjectWithTag("ChatManager").GetComponent<AIChatManager>();

        // add background message to personal info
        ChatMessage background = new ChatMessage();
        background.Content = string.Format("Your name is {0}. {1}", CharacterName, CharacterBackground);
        background.Role = "system";
        personalInfo.Add(background);
    }

    public void EnteredConversation()
    {
        inConversation = true;
        ChatMessage talk = new ChatMessage();
        talk.Content = "You are now talking to Ansh";
        talk.Role = "system";
        personalInfo.Add(talk);
        coroutine = Thinking();
        StartCoroutine(coroutine);
        responding = false;
    }

    public void ExitedConversation()
    {
        inConversation = false;
        ChatMessage talk = new ChatMessage();
        talk.Content = "You are no longer talking to Ansh";
        talk.Role = "system";
        personalInfo.Add(talk);
        StopCoroutine(coroutine);
        coroutine = null;
    }

    public void Alert(string update)
    {
        ChatMessage world = new ChatMessage();
        world.Content = update + "This happened at time " + World.instance.GetTimeStr();
        world.Role = "system";
        personalInfo.Add(world);

        //if (inConversation)
            //GameObject.FindGameObjectWithTag("ChatManager").GetComponent<AIChatManager>().Speak(personalInfo);
    }

    public void Interact()
    {
        chatManager.StartConversation(this);
    }

    public void Chat(ChatMessage message)
    {
        personalInfo.Add(message);
        Speak();
    }

    public void Speak()
    {
        if (!responding) {
            responding = true;
            chatManager.Speak(personalInfo);
        }
    }

    public void AddResponse(ChatMessage response)
    {
        personalInfo.Add(response);
        responding = false;
    }

    IEnumerator Thinking()
    {
        while (true)
        {
            // should NPC speak?
            float rand = UnityEngine.Random.Range(0f, 1f);
            Debug.Log(rand);
            if (rand <= talkative)
            {
                Speak();
            }
            yield return new WaitForSeconds(Mathf.Max(SAFEGUARD, thinkDelay));
        }
    }
}
