using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LLMUnity;

public class BasicNPC : MonoBehaviour
{
    // -- Serialize Fields --

    [SerializeField]
    DialogueManager dialogueManager;

    // -- Private Fields --
    public LLM llm;

    void Start()
    {
        Game("Hello Bot!");
    }

    void Update()
    {
        
    }

    // ----- LLM Related Functions -----

    void HandleReply(string reply)
    {
        Debug.Log("HandleReply TRIGGERED");
        dialogueManager.DisplayOutput(reply);
    }

    public void Game(string input)
    {
        Debug.Log("Game TRIGGERED");
        _ = llm.Chat(input, HandleReply);
    }
}
