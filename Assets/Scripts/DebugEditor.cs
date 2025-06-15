using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugEditor : EditorWindow
{
    private static EditorWindow instance;

    private ListView charList;
    private ListView messageList;

    public static void OpenDebugWindow()
    {
        if (instance == null)
        {
            EditorWindow wnd = GetWindow<DebugEditor>();
            wnd.titleContent = new GUIContent("Debug Window");
            instance = wnd;
        }
    }

    public static void CloseDebugWindow()
    {
        if (instance != null)
        {
            instance.Close();
            instance = null;
        }
    }

    public void CreateGUI()
    {
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        rootVisualElement.Add(splitView);

        charList = new ListView();
        splitView.Add(charList);

        messageList = new ListView();
        messageList.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
        splitView.Add(messageList);

        AICharacter[] npcs = FindObjectsOfType<AICharacter>(false);

        charList.makeItem = () => new Label();
        charList.bindItem = (item, index) => { (item as Label).text = npcs[index].name; };
        charList.itemsSource = npcs;
        charList.selectionChanged += OnCharacterSelectionChange;

        rootVisualElement.schedule.Execute(messageList.Rebuild).Every(100);
    }

    public void OnGUI()
    {
        //messageList.Rebuild();
        //Debug.Log("Rebuilding");
    }

    private void OnCharacterSelectionChange(IEnumerable<object> selectedItems)
    {
        messageList.Clear();

        var enumerator = selectedItems.GetEnumerator();
        if (enumerator.MoveNext())
        {
            AICharacter selected = enumerator.Current as AICharacter;
            if (selected != null)
            {
                messageList.makeItem = () => new Label();
                messageList.bindItem = (item, index) => (item as Label).text = AICharacter.ConvertToString(selected.shortMem[index]);
                messageList.itemsSource = selected.shortMem;
            }

        }
    }

    void Update()
    {
    }

    public void Dumb()
    {
        //label.text += "!";
    }

    public void Test()
    {
        Debug.Log("IT WORKED!");
    }

}
