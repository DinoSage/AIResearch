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

    // -- Private Fields --
    BasicNPC currentNPC;
    bool dialogueActive;


    void Start()
    {
        UIGUI.SetActive(false);
        currentNPC = null;
        dialogueActive = false;
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
            string input = inputText.text;
            inputText.text = "";
            currentNPC.Chat(input);
            Debug.Log("SUBMITTED!");
        }
    }

    public void OnEnd()
    {
        EndConversation();
    }
}
