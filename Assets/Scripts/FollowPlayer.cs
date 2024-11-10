using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    void Update()
    {
        Vector3 pos = Player.instance.transform.position;
        pos.z = this.transform.position.z;
        this.transform.position = pos;
    }
}
