using LLMUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // -- Serialize Fields --

    [SerializeField]
    GameObject UIGUI;

    [SerializeField]
    GameObject player;

    [SerializeField]
    TextMeshProUGUI outputText;

    [SerializeField]
    TextMeshProUGUI inputText;

    [SerializeField]
    GameObject[] npcs;

    [SerializeField]
    LLM llm;

    // -- Private Fields --
    BasicNPC currentNPC;
    bool dialogueActive;


    void Start()
    {
        UIGUI.SetActive(false);
        currentNPC = null;
        dialogueActive = false;

        while(!llm.serverListening)
        {
            Debug.Log("waiting");
        }
    }

    // ----- Public Functions -----

    public void DisplayOutput(string output)
    {
        outputText.SetText(output);
    }

    public void DisplayInput(string input) 
    { 
        inputText.SetText(input);
    }

    public bool BeginConversation(GameObject npc)
    {
        currentNPC = npc.GetComponent<BasicNPC>();
        UIGUI.SetActive(true);
        dialogueActive = true;
        return true;
    }

    public void EndConversation()
    {
        UIGUI.SetActive(false);
        player.GetComponent<ManageInteractions>().ResumeMovemnt();
        dialogueActive = false;
    }

    public void OnSubmit()
    {
        if (dialogueActive)
        {
            Debug.Log("SUBMITTED!");
            string input = inputText.text;
            inputText.SetText(". . . ");
            foreach (GameObject npc in npcs)
            {
                if (currentNPC.gameObject != npc)
                {
                    npc.GetComponent<BasicNPC>().Listen(input);
                }
            }
            currentNPC.Chat(input);
        }
    }

    public void OnEnd()
    {
        EndConversation();
    }
}
