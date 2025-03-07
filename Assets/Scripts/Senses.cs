using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senses : MonoBehaviour
{

    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        this.GetComponent<SpeechBubble>().Display("Who is there? I am blind!");
    }
}
