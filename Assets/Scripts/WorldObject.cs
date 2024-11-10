using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WorldObject : MonoBehaviour
{
    // -- Serialize Fields --
    [SerializeField]
    private bool debugUpdates;

    [SerializeField]
    private bool prefix;

    // -- Private Fields --
    private Setting setting;

    private string Process(string update)
    {
        string time = World.instance.GetTimeStr();
        string location = (setting == null) ? "Unknown" : setting.name;
        string str = (prefix) ? string.Format("At time {0} at {1}: {2}", time, location, update) : update;
        if (debugUpdates) Debug.Log(str);

        return str;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Setting location = collider.gameObject.GetComponent<Setting>();
        if (location != null)
        {
            this.setting = location;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        Setting location = collider.gameObject.GetComponent<Setting>();
        if (location != null && this.setting == location)
        {
            this.setting = null;
        }
    }

    // -- Events (To Subscribe To) --
    public delegate void ProxemUpdate(string update);
    public event ProxemUpdate Proxem1Trigger;
    public event ProxemUpdate Proxem2Trigger;
    public event ProxemUpdate Proxem3Trigger;

    // -- Public Functions --
    public void UpdateProxem1(string update)
    {
        Proxem1Trigger?.Invoke(Process(update));
    }

    public void UpdateProxem2(string update)
    {
        Proxem2Trigger?.Invoke(Process(update));
    }

    public void UpdateProxem3(string update)
    {
        Proxem3Trigger?.Invoke(Process(update));
    }

    public void UpdateProxemAll(string update)
    {
        UpdateProxem1(update);
        UpdateProxem2(update);
        UpdateProxem3(update);
    }
}
