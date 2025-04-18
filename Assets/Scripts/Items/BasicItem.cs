using UnityEngine;

public class BasicItem : MonoBehaviour, IInteractable
{
    [TextAreaAttribute][SerializeField] string outputText;
    [SerializeField] string proxem1Text;
    [SerializeField] string proxem2Text;
    [SerializeField] string proxem3Text;


    private WorldObject wobj;
    private OutputOptionsManager outputOptionsManager;

    private void Start()
    {
        wobj = GetComponent<WorldObject>();
        outputOptionsManager = FindAnyObjectByType<OutputOptionsManager>();
    }

    public void Interact()
    {
        outputOptionsManager.SetOutputDetails(outputText);
        wobj.UpdateProxem1(proxem1Text);
        wobj.UpdateProxem2(proxem2Text);
        wobj.UpdateProxem3(proxem3Text);
    }
}
