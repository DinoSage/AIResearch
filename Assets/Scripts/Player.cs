using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static GameObject instance;

    void Start()
    {
        instance = this.gameObject;
    }
}
