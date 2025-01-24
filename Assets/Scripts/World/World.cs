using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConversationManager;

public class World : MonoBehaviour
{
    // -- Singleton Setup --
    public static World instance;

    // -- Serialize Fields --
    [SerializeField]
    private bool debugEnabled;

    [SerializeField]
    private float secsPerGameMin;

    [SerializeField]
    public Text[] worldBackground;

    public List<ChatMessage> worldInfo = new List<ChatMessage>();

    // -- Structs --
    [Serializable]
    public struct Text
    {
        [SerializeField]
        [TextArea(3, 10)]
        public string message;
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // convert serialized world background into list of ChatMessages
        foreach (Text info in worldBackground)
        {
            ChatMessage message = new ChatMessage();
            message.Content = info.message;
            message.Role = "system";
            worldInfo.Add(message);
            Debug.Log(info.message);
        }
    }

    // -- Public Functions --
    public int[] GetTimeInts()
    {
        float factor = (secsPerGameMin <= 0) ? 10 : secsPerGameMin;
        float minutesF = Time.time / factor;
        int hour = (int)(minutesF / 60) % 24;
        int minutes = (int)(minutesF % 60);

        int[] time = { hour, minutes };
        return time;
    }

    public string GetTimeStr()
    {
        int[] time = GetTimeInts();
        return string.Format("{0:D2}:{1:D2}", time[0], time[1]);
    }

    public bool IsDebugEnabled()
    {
        return debugEnabled;
    }
}
