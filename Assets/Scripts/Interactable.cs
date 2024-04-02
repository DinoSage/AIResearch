using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // -- Serialize Fields --

    [SerializeField]
    GameObject player;

    // -- Private Fields --
    ManageInteractions interactions;

    void Start()
    {
        interactions = player.GetComponent<ManageInteractions>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            interactions.NPCDetected(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            interactions.NPCLost(collision.gameObject);
        }
    }
}
