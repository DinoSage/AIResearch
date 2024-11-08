using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WorldObject : MonoBehaviour
{

    [NonSerialized]
    public string location;

    [SerializeField]
    private bool debugUpdates;

    [SerializeField]
    private bool prefix;

    private TimeManager time;
    void Start()
    {
        time = FindFirstObjectByType<TimeManager>();
    }

    private string PrefixDetails(string update)
    {
        string str = (prefix) ? string.Format("At time {0} at {1}: {2}", time.GetTimeStr(), location, update) : update;
        if (debugUpdates) Debug.Log(str);

        return str;
    }

    public delegate void ProxemUpdate(string update);

    public event ProxemUpdate Proxem1Trigger;
    public event ProxemUpdate Proxem2Trigger;
    public event ProxemUpdate Proxem3Trigger;

    public void UpdateProxem1(string update)
    {
        Proxem1Trigger?.Invoke(PrefixDetails(update));
    }

    public void UpdateProxem2(string update)
    {
        Proxem2Trigger?.Invoke(PrefixDetails(update));
    }

    public void UpdateProxem3(string update)
    {
        Proxem3Trigger?.Invoke(PrefixDetails(update));
    }

    public void UpdateProxemAll(string update)
    {
        UpdateProxem1(update);
        UpdateProxem2(update);
        UpdateProxem3(update);
    }
}
