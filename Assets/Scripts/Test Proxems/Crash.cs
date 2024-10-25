using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    [SerializeField]
    private float crashDelay;

    private void Start()
    {
        StartCoroutine(Break());
    }

    IEnumerator Break()
    {
        yield return new WaitForSeconds(crashDelay);
        WorldObject wobj = this.GetComponent<WorldObject>();
        wobj.UpdateProxem3("Max accidently knocked over a really expensive vase.");
        wobj.UpdateProxem2("A really expensive vase crashed. No clue who broke it or how it happened.");
        wobj.UpdateProxem1("Some object broke. No clue what broke or how it happened.");
        Debug.Log("CRASHED!!!");
    }
}
