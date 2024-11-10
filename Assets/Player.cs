using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static GameObject player;

    void Start()
    {
        player = this.gameObject;
    }
}
