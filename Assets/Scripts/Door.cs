using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    // -- Serialize Fields --
    [SerializeField]
    private Door otherDoor;

    [SerializeField]
    private Vector2 offset;

    public Vector2 GetOutputPosition()
    {
        return this.transform.position + new Vector3(offset.x, offset.y, 0);
    }

    public void Interact()
    {
        Player.instance.transform.position = otherDoor.GetOutputPosition();
    }
}
