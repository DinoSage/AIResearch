using UnityEngine;
using UnityEngine.Events;

public class BasicItem : MonoBehaviour, IInteractable
{
    [TextAreaAttribute][SerializeField] string outputText;
    [SerializeField] string proxem1Text;
    [SerializeField] string proxem2Text;
    [SerializeField] string proxem3Text;

    [SerializeField] bool hideable;


    private WorldObject wobj;
    private OutputOptionsManager outputOptionsManager;

    private void Start()
    {
        wobj = GetComponent<WorldObject>();
        outputOptionsManager = FindAnyObjectByType<OutputOptionsManager>();
    }

    public void Interact()
    {
        if (hideable)
        {
            string[] optionNames = { "Hide" };
            UnityAction[] optionActions = { Hide };

            outputOptionsManager.SetOutputDetails(outputText, optionNames, optionActions);
        }
        else
        {
            outputOptionsManager.SetOutputDetails(outputText);
        }
        wobj.UpdateProxem1(proxem1Text);
        wobj.UpdateProxem2(proxem2Text);
        wobj.UpdateProxem3(proxem3Text);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
