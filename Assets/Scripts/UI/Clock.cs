using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    // -- Private Fields --
    private TextMeshProUGUI clock;

    void Start()
    {
        clock = this.GetComponent<TextMeshProUGUI>();   
    }

    void Update()
    {
        string time = World.instance.GetTimeStrClock();
        clock.SetText(time);
    }
}
