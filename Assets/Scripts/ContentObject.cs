using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class ContentObject
{
    // Constructors
    public ContentObject() { }

    public ContentObject(string category, string message, string time = "", string location = "", string character = "")
    {
        Category = category;
        Time = time;
        Location = location;
        Character = character;
        Message = message;
    }

    private static readonly string PATTERN = @"\[(?<category>\w+)(?: \| date=(?<date>\d{4}-\d{2}-\d{2}))?(?: \| time=(?<time>\d{2}:\d{2}))?(?: \| location=(?<location>[\w\s]+?))?(?: \| character=(?<npc>[\w\s]+?))?\](?: (?<message>.*))?";
    public string Category { get; set; } // Required
    public string Date { get; set; } // Optional (Format: YYYY-MM-DD)
    public string Time { get; set; } // Optional (Format: HH:MM)
    public string Location { get; set; } // Optional
    public string Character { get; set; } // Optional
    public string Message { get; set; } // Required

    public static ContentObject StringToObject(string content)
    {
        ContentObject obj = new ContentObject();
        Match match = Regex.Match(content, PATTERN);
        obj.Category = match.Groups["category"].Value;
        obj.Date = match.Groups["date"].Value;
        obj.Time = match.Groups["time"].Value;
        obj.Location = match.Groups["location"].Value;
        obj.Character = match.Groups["npc"].Value;
        obj.Message = match.Groups["message"].Value;

        return obj;
    }

    public static string ObjectToString(ContentObject obj)
    {
        string formattedString = $"[{obj.Category}";

        if (!string.IsNullOrEmpty(obj.Date))
            formattedString += $" | date={obj.Date}";

        if (!string.IsNullOrEmpty(obj.Time))
            formattedString += $" | time={obj.Time}";

        if (!string.IsNullOrEmpty(obj.Location))
            formattedString += $" | location={obj.Location}";

        if (!string.IsNullOrEmpty(obj.Character))
            formattedString += $" | character={obj.Character}";

        formattedString += $"] {obj.Message}";

        return formattedString;
    }
}
