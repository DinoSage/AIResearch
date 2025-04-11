using TMPro;
using UnityEngine;

public class Map : MonoBehaviour
{
    // -- Private Fields --
    private TextMeshProUGUI map;
    private Locator playerLocator;

    void Start()
    {
        map = this.GetComponent<TextMeshProUGUI>();
        playerLocator = Player.instance.GetComponent<Locator>();
    }

    void Update()
    {
        string location = playerLocator.GetCurrSettingName();
        map.SetText(location);
    }
}
