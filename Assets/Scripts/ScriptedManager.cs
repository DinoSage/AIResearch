using OpenAI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScriptedManager : MonoBehaviour
{
    // -- Serialize Fields --
    [SerializeField]
    GameObject chatUI;

    [SerializeField]
    TextMeshProUGUI output;

    // -- Fields --
    private ScriptedNPC currentNPC;

    public async void ChatNPC()
    {
        if (currentNPC == null)
        {
            Debug.LogWarning("GPT: current BasicNPC is null");
            return;
        }

        string line = currentNPC.NextDialogue();

        if (line.Equals("|END|"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Interact>().OnEnd();
        } else
        {
            output.SetText(line);

        }

    }

    // -- Methods --
    public void StartConversation(GameObject npc)
    {
        chatUI.SetActive(true);
        currentNPC = npc.GetComponent<ScriptedNPC>();
    }

    public void EndConversation()
    {
        chatUI.SetActive(false);
        currentNPC = null;
        output.SetText("");
    }
}
