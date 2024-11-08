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
        clock.SetText(timeManager.GetTimeStr());
    }
}
