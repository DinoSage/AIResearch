using UnityEngine;

public class Locator : MonoBehaviour
{
    // -- Private Variables --
    private Setting currSetting;

    private void Update()
    {
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
    public string GetCurrSetting()
    {
        return (currSetting != null) ? currSetting.GetSettingName() : "Unknown";
    }
}
