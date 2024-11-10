using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // -- Serialize Fields --
    [SerializeField]
    private Door door;

    [SerializeField]
    private Vector2 offset;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = door.GetOutputPosition();
        }
    }

    public Vector2 GetOutputPosition()
    {
        return this.transform.position + new Vector3(offset.x, offset.y, 0);
    }
}
