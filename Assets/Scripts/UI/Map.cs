using TMPro;
using UnityEngine;

public class Map : MonoBehaviour
{
    // -- Private Fields --
    private TextMeshProUGUI map;

    void Start()
    {
        map = this.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        string location = "";
        map.SetText(location);
    }
}
