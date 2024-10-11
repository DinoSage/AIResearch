using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.Rendering.CameraUI;

public class AICharacter : MonoBehaviour, ICharacter
{
    // -- Serialize Fields --
    [SerializeField]
    private string CharacterName;

    [SerializeField]
    [TextArea(3, 10)]
    private string CharacterBackground;

    // -- Non-Serialized Fields -- 
    [NonSerialized] public List<ChatMessage> personalInfo = new List<ChatMessage>();


    // -- Functions --
    void Start()
    {
        ChatMessage background = new ChatMessage();
        background.Content = string.Format("Your name is {0}. {1}", CharacterName, CharacterBackground);
        background.Role = "system";
        personalInfo.Add(background);
    }
}
