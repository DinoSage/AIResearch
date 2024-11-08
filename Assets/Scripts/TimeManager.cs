using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    float secsPerGameMin;

    public int[] GetTimeInts()
    {
        float factor = (secsPerGameMin <= 0) ? 10 : secsPerGameMin;
        float minutesF = Time.time / factor;
        int hour = (int) (minutesF / 60) % 24;          
        int minutes = (int) (minutesF % 60);

        int[] time = {hour, minutes};
        return time; 
    }

    public string GetTimeStr()
    {
        int[] time = GetTimeInts();
        return string.Format("{0:D2}:{1:D2}", time[0], time[1]);
    }
}
