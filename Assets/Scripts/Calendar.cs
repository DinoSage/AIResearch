using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Calendar : MonoBehaviour
{
    // -- Private Fields --
    private TextMeshProUGUI clock;

    void Start()
    {
        clock = this.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        string date = World.instance.GetDateStrCalendar();
        clock.SetText(date);
    }
}
