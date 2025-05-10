using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Locator : MonoBehaviour
{
    // -- Private Variables --
    [SerializeField] private Setting currSetting;

    private void Update()
    {
        /*string destinations = "";
        foreach (Setting dest in currSetting.GetAllDestinations())
        {
            destinations += dest.GetSettingName() + "\t";
        }
        Debug.Log("Destinations: " + destinations);*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Setting"))
        {
            currSetting = collision.gameObject.GetComponent<Setting>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Setting"))
        {
            currSetting = null;
        }
    }

    // -- Public Function --
    public string GetCurrSettingName()
    {
        return (currSetting != null) ? currSetting.GetSettingName() : "Unknown";
    }

    public Setting GetCurrSetting()
    {
        return currSetting;
    }
}
