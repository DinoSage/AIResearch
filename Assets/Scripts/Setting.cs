using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Setting : MonoBehaviour
{
    // -- Private Fields --
    private List<WorldObject> characters = new List<WorldObject>();
    private List<WorldObject> doors = new List<WorldObject>();
    private List<WorldObject> items = new List<WorldObject>();

    // -- Internal --

    private void Start()
    {
        /*Bounds bounds = this.GetComponent<SpriteRenderer>().bounds;
        Collider2D[] hits = Physics2D.OverlapBoxAll(this.transform.position, bounds.size, 0f, LayerMask.GetMask("Door"));
        doors = new Door[hits.Length];
        Debug.Log($"For {name}:");
        for (int i = 0; i < hits.Length; i++)
        {
            doors[i] = hits[i].gameObject.GetComponent<Door>();
            Debug.Log($"Door To: {doors[i].GetDestination() == null}");
        }*/
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        WorldObject wobj = collision.GetComponent<WorldObject>();
        if (wobj != null)
        {
            BasicItem item = wobj.GetComponent<BasicItem>();
            Door door = wobj.GetComponent<Door>();
            SpeechBubble bubble = wobj.GetComponent<SpeechBubble>();

            if (item != null)
            {
                items.Add(wobj);
            }

            if (bubble != null)
            {
                characters.Add(wobj);
            }

            if (door != null)
            {
                doors.Add(wobj);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        WorldObject wobj = collision.GetComponent<WorldObject>();
        if (wobj != null)
        {
            BasicItem item = wobj.GetComponent<BasicItem>();
            Door door = wobj.GetComponent<Door>();
            SpeechBubble bubble = wobj.GetComponent<SpeechBubble>();

            if (item != null)
            {
                items.Remove(wobj);
            }

            if (bubble != null)
            {
                characters.Remove(wobj);
            }

            if (door != null)
            {
                doors.Remove(wobj);
            }
        }
    }

    // -- Public Functions --
    public Vector2 RandPointInSetting()
    {
        float posX = Random.Range(-1 * 20 / 2, 20 / 2);
        float posY = Random.Range(-1 * 20 / 2, 20 / 2);
        return new Vector2(posX, posY);
    }

    public string GetSettingName()
    {
        return this.gameObject.name;
    }

    public WorldObject[] GetItems()
    {

        return items.ToArray();
    }

    public WorldObject[] GetCharacters()
    {
        return characters.ToArray();
    }

    public WorldObject[] GetDoors()
    {
        return doors.ToArray();
    }

    public WorldObject[] GetAll()
    {
        return items.Concat(characters).Concat(doors).ToArray();
    }
}
