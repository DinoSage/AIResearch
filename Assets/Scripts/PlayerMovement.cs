using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // -- Serialize Fields --
    [SerializeField]
    float speed;

    // -- Non-Serialized Fields --
    private bool canMove = true;
    private InputAction moveAction;

    void Start()
    {
        // find move action for movement input in update
        PlayerInput input = this.GetComponent<PlayerInput>();
        moveAction = input.actions.FindAction("Move");
    }

    void Update()
    {
        // move player each frame based on movement input (if movement enabled)
        if (canMove)
        {
            Vector2 deltaPos = moveAction.ReadValue<Vector2>().normalized;
            deltaPos = deltaPos * speed * Time.deltaTime;

            this.transform.position += new Vector3(deltaPos.x, deltaPos.y, 0);
        }
    }

    public void OnTest()
    {
        this.GetComponent<WorldObject>().UpdateProxemAll("The person named Ansh coughed.");
    }

}
