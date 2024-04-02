using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageInteractions : MonoBehaviour
{
    // -- Private Fields --
    List<GameObject> npcs;

    void Start()
    {
        npcs = new List<GameObject>();        
    }

    void Update()
    {
        string str = "";
        foreach (GameObject npc in npcs)
        {
            str += npc.name + " ";
        }
        Debug.Log(str);
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
}
