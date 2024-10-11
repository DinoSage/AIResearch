using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedNPC : MonoBehaviour
{
    [SerializeField]
    string name;

    [SerializeField]
    Dialogue dialogue;

    // private
    int count = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string NextDialogue()
    {
        count++;

        if (count >= dialogue.sentences.Length)
        {
            count = -1;
            return "|END|";
        } else
        {
            return dialogue.sentences[count];
        }
    }
}
