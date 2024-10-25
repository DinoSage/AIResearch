using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChat
{
    public void Speak();
    public void Respond();

    public void StartConversation(ICharacter npc);

    public void EndConversation();
}
