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
    public event ProxemUpdate Proxem1Trigger;
    public event ProxemUpdate Proxem2Trigger;
    public event ProxemUpdate Proxem3Trigger;

    // -- Public Functions --
    public void UpdateProxem1(string update)
    {
        Proxem1Trigger?.Invoke(update);
    }

    public void UpdateProxem2(string update)
    {
        Proxem2Trigger?.Invoke(update);
    }

    public void UpdateProxem3(string update)
    {
        Proxem3Trigger?.Invoke(update);
    }

    public void UpdateProxemAll(string update)
    {
        UpdateProxem1(update);
        UpdateProxem2(update);
        UpdateProxem3(update);
    }
}
