using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locale : MonoBehaviour
{
    // -- Private Fields --
    private Setting currLocale;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Setting setting = collision.GetComponent<Setting>();
        if (setting != null)
        {
            currLocale = setting;
            Debug.Log(setting.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Setting setting = collision.GetComponent<Setting>();
        if (setting == currLocale)
        {
            currLocale = null;
        }
    }

    public Setting GetCurrLocale() 
    { 
        return currLocale; 
    }
}
