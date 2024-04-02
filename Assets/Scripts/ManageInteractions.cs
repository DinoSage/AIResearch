using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageInteractions : MonoBehaviour
{
    // -- Serialize Fields --

    [SerializeField]
    DialogueManager dialogueManager;
    
    // -- Private Fields --
    List<GameObject> npcs;

    void Start()
    {
        npcs = new List<GameObject>();        
    }

    void Update()
    {
    }

    public void OnInteract()
    {
        // find closest npc
        float closDist = float.MaxValue;
        GameObject closNPC = null;

        foreach (GameObject npc in npcs)
        {
            float dist = Vector3.Distance(this.transform.position, npc.transform.position);
            if (dist < closDist)
            {
                dist = closDist;
                closNPC = npc;
            }
        }

        // start conversation
        bool check = dialogueManager.BeginConversation(closNPC);
        this.GetComponent<PlayerMovement>().enabled = false;

        Debug.Log("Talking to: [ " + closNPC.name + " ]");
    }


    // -- Public Functions --

    public void NPCDetected(GameObject npc)
    {
        if (npcs.IndexOf(npc) == -1)
        {
            npcs.Add(npc);
        }
    }

    public void NPCLost(GameObject npc)
    {
        npcs.Remove(npc);
    }

    public void ResumeMovemnt()
    {
        this.GetComponent<PlayerMovement>().enabled = true;
        Debug.Log("No longer talking!");
    }
}
