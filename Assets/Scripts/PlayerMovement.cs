using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // -- Serialize Fields --
    [SerializeField]
    float speed;

    [SerializeField]
    private AICharacter[] test;

    // -- Non-Serialized Fields --
    private bool canMove = true;
    private InputAction moveAction;
    private WorldObject wobj;
    private Vector3 prevPos;
    private bool isMoving = false;

    void Start()
    {
        // find move action for movement textInput in update
        PlayerInput input = this.GetComponent<PlayerInput>();
        wobj = GetComponent<WorldObject>();
        moveAction = input.actions.FindAction("Move");
        prevPos = this.transform.position;

        //StartCoroutine(Walking());
    }

    void Update()
    {
        // move player each frame based on movement textInput (if movement enabled)
        if (canMove)
        {
            Vector2 deltaPos = moveAction.ReadValue<Vector2>().normalized;
            deltaPos = deltaPos * speed * Time.deltaTime;

            this.transform.position += new Vector3(deltaPos.x, deltaPos.y, 0);
        }

        ContentObject cobjFar = new ContentObject("EVENT", "You can see Ansh is stopped at a distance. You can only see him.");
        cobjFar.Time = World.instance.GetTimeStrAI();
        cobjFar.Date = World.instance.GetDateStrAI();

        ContentObject cobjClose = new ContentObject("EVENT", "You can see and hear Ansh stopped right next to you. You can see, hear, and speak with him.");
        cobjClose.Time = World.instance.GetTimeStrAI();
        cobjClose.Date = World.instance.GetDateStrAI();


        if (Vector3.Distance(this.transform.position, prevPos) > 0 && !isMoving)
        {
            Debug.Log("STARTED MOVING!");
        }
        else if (Vector3.Distance(this.transform.position, prevPos) == 0 && isMoving)
        {
            Debug.Log("STOPPED MOVING!");
            wobj.UpdateProxem2(cobjFar.ToString());
            wobj.UpdateProxem3(cobjClose.ToString());
        }

        isMoving = (Vector3.Distance(this.transform.position, prevPos) > 0);
        prevPos = this.transform.position;
    }

    public void OnTest()
    {
        this.GetComponent<WorldObject>().UpdateProxemAll("The person named Ansh coughed.");
    }

    /*IEnumerator Walking()
    {
        while (true)
        {
            if (Mathf.Abs(moveAction.ReadValue<Vector2>().magnitude) > 0)
            {
                wobj.UpdateProxem3("You see Ansh");
                yield return new WaitForSeconds(1);
            }
        }
    }*/

}
