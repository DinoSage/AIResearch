using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    TimeManager timeManager;

    // -- Private Fields --
    private TextMeshProUGUI clock;

    void Start()
    {
        clock = this.GetComponent<TextMeshProUGUI>();   
    }

    void Update()
    {
        int[] time = timeManager.GetTime();
        clock.SetText(string.Format("{0:D2}:{1:D2}", time[0], time[1]));
    }
}
