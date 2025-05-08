using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WorldObject : MonoBehaviour
{
    // -- Serialize Fields --
    [SerializeField]
    private string Proxem1Desc;
    [SerializeField]
    private string Proxem2Desc;
    [SerializeField]
    private string Proxem3Desc;

    // -- Events (To Subscribe To) --
    public delegate void ProxemUpdate(string update);
    public event ProxemUpdate outerProxemTrigger;
    public event ProxemUpdate middleProxemTrigger;
    public event ProxemUpdate innerProxemTrigger;

    // -- Public Functions --
    public void UpdateOuterProxem(string update)
    {
        outerProxemTrigger?.Invoke(update);
    }

    public void UpdateMiddleProxem(string update)
    {
        middleProxemTrigger?.Invoke(update);
    }

    public void UpdateInnerProxem(string update)
    {
        innerProxemTrigger?.Invoke(update);
    }

    public void UpdateProxemAll(string update)
    {
        UpdateOuterProxem(update);
        UpdateMiddleProxem(update);
        UpdateInnerProxem(update);
    }
}
