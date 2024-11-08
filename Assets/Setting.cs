using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField]
    private string settingName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        WorldObject wo = collision.GetComponent<WorldObject>();
        if (wo != null)
        {
            wo.location = settingName;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        WorldObject wo = collision.GetComponent<WorldObject>();
        if (wo != null)
        {
            wo.location = "Unknown";
        }
    }


}
