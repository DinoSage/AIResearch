using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;

public class BasicNPC : MonoBehaviour
{
    // -- Serialize Fields --

    [SerializeField]
    GameObject dialogueSystem;

    // -- Private Fields --
    DialogueManager dialogueManager;
    LLMClient client;

    void Start()
    {
        dialogueManager = dialogueSystem.GetComponent<DialogueManager>();
        client = GetComponent<LLMClient>();
    }

    void Update()
    {
        
    }

    // ----- LLM Related Functions -----

    public void HandleReply(string reply)
    {
        dialogueManager.DisplayOutput(reply);
    }

    public void Chat(string input)
    {
        _ = client.Chat(input, HandleReply);
    }
}
