using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    // -- Serialize Fields --
    [SerializeField]
    float interactRadius;

    // -- Non-Serialized Fields --
    private bool inConversation = false;
    private IChat chatManager;

    // -- Functions --
    private void Start()
    {
        chatManager = GameObject.FindGameObjectWithTag("ChatManager").GetComponent<IChat>();
    }

    public void OnInteract()
    {
        if (!inConversation)
        {
            // find closest npc within range
            Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, interactRadius);
            GameObject closest = null;
            float minDist = Mathf.Infinity;
            foreach (Collider2D hit in hits)
            {
                float dist = Vector2.Distance(hit.gameObject.transform.position, this.transform.position);
                if (minDist > dist)
                {
                    minDist = dist;
                    closest = hit.gameObject;
                }
            }

            // initiate conversation if npc exists
            if (closest != null)
            {
                chatManager.StartConversation(closest.GetComponent<AICharacter>());
                PlayerMovement.canMove = false;
                inConversation = true;
            } else
            {
                Debug.Log("INFO: no NPC in range");
            }
        }
    }

    public void OnSubmit()
    {
        // chat with npc if in conversation
        if (inConversation)
        {
            chatManager.Chat();
        }
    }

    public void OnEnd()
    {
        // end conversation if in conversation
        if (inConversation)
        {
            chatManager.EndConversation();
            PlayerMovement.canMove = true;
            inConversation = false;
        }
    }
}
