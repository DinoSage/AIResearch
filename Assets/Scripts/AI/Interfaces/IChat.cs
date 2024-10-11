using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChat
{
    public void Chat();

    public void StartConversation(ICharacter npc);

    public void EndConversation();
}
