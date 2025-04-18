using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Tester : MonoBehaviour
{
    private OutputOptionsManager outputOptionsManager;

    void Start()
    {
        //outputOptionsManager = FindObjectOfType<OutputOptionsManager>();
        //StartCoroutine(TestDelayOutput());
    }

    void Update()
    {

    }

    IEnumerator TestDelayOutput()
    {
        yield return new WaitForSeconds(3);
        string[] optionNames = { "Add", "Subtract", "Multiply", "Divide" };
        UnityAction[] unityActions = { Add, Subtract, Multiply, Divide };
        string text = "What is the math operation present in: 2 ^ 2?";
        outputOptionsManager.SetOutputDetails(text, optionNames, unityActions);
    }

    public void Add()
    {
        Debug.Log("ADD");
    }
    public void Subtract()
    {
        Debug.Log("SUBTRACT");
    }
    public void Multiply()
    {
        Debug.Log("Multiply");
    }
    public void Divide()
    {
        Debug.Log("Divide");
    }
}
