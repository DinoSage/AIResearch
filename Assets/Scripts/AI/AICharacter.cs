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
    // -- Serialize Fields --

    [Header("Background")]
    [SerializeField]
    private string CharacterName;

    [SerializeField]
    [TextArea(3, 10)]
    private string CharacterBackground;

    [SerializeField]
    private float talkative;

    // -- Non-Serialized Fields -- 
    [NonSerialized] public List<ChatMessage> personalInfo = new List<ChatMessage>();
    private bool inConversation = false;
    private AIChatManager chatManager;

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

    public void ConversationStarted()
    {
        inConversation = true;
        ChatMessage talk = new ChatMessage();
        talk.Content = "You are now talking to Ansh";
        talk.Role = "system";
        personalInfo.Add(talk);
    }

    public void ConversationEnded()
    {
        inConversation = false;
        ChatMessage talk = new ChatMessage();
        talk.Content = "You are no longer talking to Ansh";
        talk.Role = "system";
        personalInfo.Add(talk);
    }

    public void Alert(string update)
    {
        ChatMessage world = new ChatMessage();
        world.Content = update + "The time is " + World.instance.GetTimeStr();
        world.Role = "system";
        personalInfo.Add(world);

        if (inConversation)
            GameObject.FindGameObjectWithTag("ChatManager").GetComponent<AIChatManager>().Speak();
    }

    public void Interact()
    {
        chatManager.StartConversation(this);

    }
}
