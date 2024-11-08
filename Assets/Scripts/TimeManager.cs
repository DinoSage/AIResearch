using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    float secsPerGameMin;

    public int[] GetTime()
    {
        float factor = (secsPerGameMin <= 0) ? 10 : secsPerGameMin;
        float minutesF = Time.time / factor;
        int hour = (int) (minutesF / 60) % 24;          
        int minutes = (int) (minutesF % 60);

        int[] time = {hour, minutes};
        return time; 
    }
}
