using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // -- Serialize Fields --

    [SerializeField]
    TextMeshProUGUI output_tmp;

    [SerializeField]
    TextMeshProUGUI input_tmp;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // ----- Input Actions -----

    public void OnSubmit()
    {
        Debug.Log("Submitted");
    }
}
