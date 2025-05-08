using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senses : MonoBehaviour
{

    // -- Internal --
    private AICharacter character;

    void Start()
    {
        character = GetComponent<AICharacter>();
    }

    void Update()
    {
        
    }

    // -- External --
    
    public void Sense(string update)
    {
        Ears(update);
    }

    public void Ears(string update)
    {
        Debug.Log("SPEC: " + update);
        character.Chat(update);
    }
}
