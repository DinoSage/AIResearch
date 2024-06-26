using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // -- Serialize Fields --

    [SerializeField]
    float speed = 1;

    // -- Private Fields --
    InputAction moveAction;

    void Start()
    {
        // load input actions
        PlayerInput input = this.GetComponent<PlayerInput>();
        moveAction = input.actions.FindAction("Move");
        
    }

    void Update()
    {
        Vector2 deltaPos = moveAction.ReadValue<Vector2>().normalized;
        deltaPos = deltaPos * speed * Time.deltaTime;

        this.transform.position += new Vector3(deltaPos.x, deltaPos.y, 0);
    }

}
