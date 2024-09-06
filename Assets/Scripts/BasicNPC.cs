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

    public async void Chat(string input)
    {
        _ = await client.Chat(input, HandleReply);
    }

    public void Listen(string input)
    {
        input = "imagine you overhear that: " + input;
        _ = client.Chat(input, ListenTest);
    }

    public void ListenTest(string reply)
    {
        Debug.Log(reply);
    }
}
