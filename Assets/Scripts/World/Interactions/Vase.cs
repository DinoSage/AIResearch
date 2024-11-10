using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : MonoBehaviour, IInteractable
{
    // -- Private Variables --
    private bool isBroken = false;

    public void Interact()
    {
        if (!isBroken)
        {
            WorldObject wobj = this.GetComponent<WorldObject>();
            wobj.UpdateProxem3("Ansh accidently knocked over a really expensive vase.");
            wobj.UpdateProxem2("A really expensive vase crashed. No clue who broke it or how it happened.");
            wobj.UpdateProxem1("Some object broke. No clue what broke or how it happened.");
            isBroken = true;

            if (World.instance.IsDebugEnabled())
            {
                Debug.Log("The Vase is now broken");
            }
        } else
        {
            if (World.instance.IsDebugEnabled())
            {
                Debug.Log("The Vase was already broken");
            }
        }
    }
}
