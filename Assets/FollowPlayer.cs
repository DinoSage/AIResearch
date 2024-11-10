using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    // -- Private Fields --
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 pos = player.transform.position;
        pos.z = this.transform.position.z;
        this.transform.position = pos;
    }
}
