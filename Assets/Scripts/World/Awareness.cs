using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Awareness : MonoBehaviour
{
    // -- Serialize Fields --
    [Header("Awareness")]
    [SerializeField]
    private float proxem1Range;

    [SerializeField]
    private float proxem2Range;

    [SerializeField]
    private float proxem3Range;

    [SerializeField]
    private string updateTriggerName;

    // -- Non-Serialized Fields --
    List<WorldObject> subscribed;


    // -- Functions --
    void Awake()
    {
        // validate serialize field results
        bool ordered = (proxem1Range >= proxem2Range) && (proxem2Range >= proxem3Range);
        bool nonzero = (proxem1Range >= 0) && (proxem2Range >= 0) && (proxem3Range >= 0);
        if (!nonzero || !ordered)
        {
            Debug.LogWarning("Proxem Ranges are less than zero or not in decreasing order (range1 >= range2 >= range3)");
        }

        subscribed = new List<WorldObject>();
    }

    void Update()
    {
        foreach(WorldObject wobj in subscribed)
        {
            wobj.Proxem1Trigger -= Trigger;
            wobj.Proxem2Trigger -= Trigger;
            wobj.Proxem3Trigger -= Trigger;
        }
        subscribed.Clear();


        Collider2D[] objects = Physics2D.OverlapCircleAll(this.transform.position, proxem1Range);
        foreach (Collider2D obj in objects)
        {
            WorldObject wobj = obj.GetComponent<WorldObject>();
            if (wobj != null)
            {
                float dist = Vector2.Distance(this.transform.position, wobj.transform.position);
                if (dist <= proxem3Range)
                {
                    wobj.Proxem3Trigger += Trigger;
                } 
                else if (dist <= proxem2Range)
                {
                    wobj.Proxem2Trigger += Trigger;
                }
                else
                {
                    wobj.Proxem1Trigger += Trigger;
                }
                subscribed.Add(wobj);
            }
        }
    }

    private void Trigger(string update)
    {
        this.SendMessage(updateTriggerName, update);
    }
}
