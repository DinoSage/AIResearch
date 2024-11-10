using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Setting : MonoBehaviour
{
    [SerializeField]
    public string settingName;

    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Gizmos.color = new Color(1, 1, 1, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(collider.size.x, collider.size.y, 1));
    }
}
