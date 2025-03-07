using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class ContentObject
{
    private static readonly string CONTENT_PATTERN = @"\[(?<category>\w+)(?: \| date=(?<date>\d{4}-\d{2}-\d{2}))?(?: \| time=(?<time>\d{2}:\d{2}))?(?: \| location=(?<location>[\w\s]+?))?(?: \| character=(?<npc>[\w\s]+?))?\](?: (?<message>.*))?";

    // -- External --
    public string Category { get; set; } // Required
    public string Date { get; set; } // Optional (Format: YYYY-MM-DD)
    public string Time { get; set; } // Optional (Format: HH:MM)
    public string Location { get; set; } // Optional
    public string Character { get; set; } // Optional
    public string Message { get; set; } // Optional

    public ContentObject() { }

    public ContentObject(string category, string message, string time = "", string location = "", string character = "")
    {
        Category = category;
        Time = time;
        Location = location;
        Character = character;
        Message = message;
    }


    public static ContentObject FromString(string content)
    {
        ContentObject obj = new ContentObject();
        Match match = Regex.Match(content, CONTENT_PATTERN);
        obj.Category = match.Groups["category"].Value;
        obj.Date = match.Groups["date"].Value;
        obj.Time = match.Groups["time"].Value;
        obj.Location = match.Groups["location"].Value;
        obj.Character = match.Groups["npc"].Value;
        obj.Message = match.Groups["message"].Value;

        return obj;
    }

    public string ToString()
    {
        string formattedString = $"[{Category}";

        if (!string.IsNullOrEmpty(Date))
            formattedString += $" | date={Date}";

        if (!string.IsNullOrEmpty(Time))
            formattedString += $" | time={Time}";

        if (!string.IsNullOrEmpty(Location))
            formattedString += $" | location={Location}";

        if (!string.IsNullOrEmpty(Character))
            formattedString += $" | character={Character}";

        formattedString += $"] {Message}";

        return formattedString;
    }
}
