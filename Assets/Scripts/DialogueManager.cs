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
    BasicNPC npc;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // ----- Public Functions -----

    public void DisplayOutput(string output)
    {
        Debug.Log("Display Output TRIGGERED");
        output_tmp.SetText(output);
    }

    public void DisplayInput(string input) 
    { 
        input_tmp.SetText(input);
    }

    // ----- Input Actions -----

    public void OnSubmit()
    {
        string input = input_tmp.text;
        input_tmp.SetText("nono");
        npc.Game(input);
    }
}
