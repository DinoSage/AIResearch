using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    // -- Serialize Fields --
    [SerializeField]
    float interactRadius;

    public void OnInteract()
    {
        // find closest interactable object and interact with it
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, interactRadius);
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponent<IInteractable>() != null)
            {
                float dist = Vector2.Distance(hit.gameObject.transform.position, this.transform.position);
                if (minDist > dist)
                {
                    minDist = dist;
                    closest = hit.gameObject;
                }
            }
        }
        if (closest != null)
        {
            closest.gameObject.GetComponent<IInteractable>().Interact();
        }

        /* if (!inConversation)
         {
             // find closest npc within range
             Collider2D[] hits = Physics2D.OverlapCircleAll(this.transform.position, interactRadius, LayerMask.GetMask("NPC"));
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
         }*/
    }

    
}
