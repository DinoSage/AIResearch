using System.Collections.Generic;
using UnityEngine;

public class Awareness : MonoBehaviour
{
    // -- Serialize Fields --
    [Header("Awareness")]

    [SerializeField]
    private Proxem innerProxemity;

    [SerializeField]
    private Proxem middleProxemity;

    [SerializeField]
    private Proxem outerProxemity;

    [SerializeField]
    private string updateFunctionBroadcastName;

    // -- Non-Serialized Fields --
    List<WorldObject> subscribed;


    // -- Functions --
    void Awake()
    {
        // validate serialize field results
        bool orderedInnerMiddle = (innerProxemity.range <= middleProxemity.range) || (middleProxemity.max);
        bool orderedMiddleOuter = (middleProxemity.range <= outerProxemity.range) || (outerProxemity.max);

        if (!orderedInnerMiddle || !orderedMiddleOuter)
        {
            Debug.LogWarning("Proxem Ranges are not in increasing order");
        }

        bool nonzero = (innerProxemity.range >= 0) && (middleProxemity.range >= 0) && (outerProxemity.range >= 0);
        if (!nonzero)
        {
            Debug.LogWarning("Proxem Ranges are less than zero");
        }

        subscribed = new List<WorldObject>();
    }

    void Update()
    {
        foreach (WorldObject wobj in subscribed)
        {
            wobj.outerProxemTrigger -= Trigger;
            wobj.middleProxemTrigger -= Trigger;
            wobj.innerProxemTrigger -= Trigger;
        }
        subscribed.Clear();


        Collider2D[] objects = Physics2D.OverlapCircleAll(this.transform.position, outerProxemity.range);
        foreach (Collider2D obj in objects)
        {
            WorldObject wobj = obj.GetComponent<WorldObject>();
            if (wobj != null)
            {
                // skip yourself - do not trigger to your own update
                if (wobj.gameObject == this.gameObject) continue;

                float dist = Vector2.Distance(this.transform.position, wobj.transform.position);
                if (dist <= innerProxemity.range)
                {
                    wobj.innerProxemTrigger += Trigger;
                }
                else if (dist <= middleProxemity.range)
                {
                    wobj.middleProxemTrigger += Trigger;
                }
                else
                {
                    wobj.outerProxemTrigger += Trigger;
                }
                subscribed.Add(wobj);
            }
        }
    }

    private void Trigger(string update)
    {
        this.SendMessage(updateFunctionBroadcastName, update);
    }
}

[System.Serializable]
public struct Proxem
{
    public bool max;
    public float range;
}
