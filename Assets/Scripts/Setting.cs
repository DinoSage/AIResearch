using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Setting : MonoBehaviour
{
    // -- Serialize Fields --

    [SerializeField]
    private float width;

    [SerializeField]
    private float height;

    // -- Private Fields --
    List<WorldObject> interactables = new List<WorldObject>();

    // -- Internal --
    public void OnTriggerEnter2D(Collider2D collision)
    {
        WorldObject wobj = collision.GetComponent<WorldObject>();
        if (wobj != null)
        {
            interactables.Add(wobj);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        WorldObject wobj = collision.GetComponent<WorldObject>();
        if (wobj != null)
        {
            interactables.Remove(wobj);
        }
    }

    // -- Public Functions --
    public Vector2 RandPointInSetting()
    {
        float posX = Random.Range(-1 * width / 2, width / 2);
        float posY = Random.Range(-1 * height / 2, height / 2);
        return new Vector2(posX, posY);
    }

    public string GetSettingName()
    {
        return this.gameObject.name;
    }

    public WorldObject[] GetInteractables()
    {
        return interactables.ToArray();
    }
}
