using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class MessageObject
{
    // Constructors
    public MessageObject() { }

    public MessageObject(string category, string message, string time = "", string location = "", string npc = "")
    {
        Category = category;
        Time = time;
        Location = location;
        NPC = npc;
        Message = message;
    }

    private static readonly string PATTERN = @"\[(?<category>\w+)(?: \| time=(?<time>\d{2}:\d{2}))?(?: \| location=(?<location>[\w\s]+))?(?: \| npc=(?<npc>[\w\s]+))?\] (?<message>.+)";

    public string Category { get; set; } // Required
    public string Time { get; set; } // Optional (Format: HH:MM)
    public string Location { get; set; } // Optional
    public string NPC { get; set; } // Optional
    public string Message { get; set; } // Required

    public static MessageObject ContentToObject(string content)
    {
        MessageObject obj = new MessageObject();
        Match match = Regex.Match(content, PATTERN);
        obj.Category = match.Groups["category"].Value;
        obj.Time = match.Groups["time"].Value;
        obj.Location = match.Groups["location"].Value;
        obj.NPC = match.Groups["npc"].Value;
        obj.Message = match.Groups["message"].Value;

        return obj;
    }

    public static string ObjectToContent(MessageObject obj)
    {
        string formattedString = $"[{obj.Category}";

        if (!string.IsNullOrEmpty(obj.Time))
            formattedString += $" | time={obj.Time}";

        if (!string.IsNullOrEmpty(obj.Location))
            formattedString += $" | location={obj.Location}";

        if (!string.IsNullOrEmpty(obj.NPC))
            formattedString += $" | npc={obj.NPC}";

        formattedString += $"] {obj.Message}";

        return formattedString;
    }
}
