using LLMUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // -- Serialize Fields --

    [SerializeField]
    TextMeshProUGUI output_tmp;

    [SerializeField]
    TextMeshProUGUI input_tmp;

    [SerializeField]
    BasicNPC current_npc;

    void Start()
    {

    }

    void Update()
    {
        
    }

    // ----- Public Functions -----

    public void DisplayOutput(string output)
    {
        output_tmp.SetText(output);
    }

    public void DisplayInput(string input) 
    { 
        input_tmp.SetText(input);
    }

    public void OnSubmit()
    {
        string input = input_tmp.text;
        input_tmp.text = "";
        current_npc.Chat(input);
        Debug.Log("SUBMITTED!");
    }
}
