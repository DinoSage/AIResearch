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
    private List<WorldObject> interactables = new List<WorldObject>();
    private Door[] doors;

    // -- Internal --

    private void Start()
    {
        Bounds bounds = this.GetComponent<SpriteRenderer>().bounds;
        Collider2D[] hits = Physics2D.OverlapBoxAll(this.transform.position, bounds.size, 0f, LayerMask.GetMask("Door"));
        doors = new Door[hits.Length];
        Debug.Log($"For {name}:");
        for (int i = 0; i < hits.Length; i++)
        {
            doors[i] = hits[i].gameObject.GetComponent<Door>();
            Debug.Log($"Door To: {doors[i].GetDestination() == null}");
        }
    }

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

    public Setting[] GetAllDestinations()
    {
        Setting[] settings = new Setting[doors.Length];
        for (int i = 0; i < doors.Length; i++)
        {
            settings[i] = doors[i].GetDestination();
        }

        return settings;
    }
}
