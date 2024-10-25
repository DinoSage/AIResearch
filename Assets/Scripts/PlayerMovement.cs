using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // -- Serialize Fields --
    [SerializeField]
    float speed = 1;

    // -- Fields --
    public static bool canMove = true;
    InputAction moveAction;

    void Start()
    {
        // load input actions
        PlayerInput input = this.GetComponent<PlayerInput>();
        moveAction = input.actions.FindAction("Move");
    }

    void Update()
    {
        if (canMove)
        {
            Vector2 deltaPos = moveAction.ReadValue<Vector2>().normalized;
            deltaPos = deltaPos * speed * Time.deltaTime;

            this.transform.position += new Vector3(deltaPos.x, deltaPos.y, 0);
        }
    }

    public void OnTest()
    {
        this.GetComponent<WorldObject>().UpdateProxem1("Hellow from playe rmovemnt!");
    }
}
