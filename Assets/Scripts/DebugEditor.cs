using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugEditor : EditorWindow
{
    private static EditorWindow instance;

    private TwoPaneSplitView splitView;
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
        splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        rootVisualElement.Add(splitView);

        charList = new ListView();
        splitView.Add(charList);

        messageList = new ListView();
        messageList.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
        splitView.Add(messageList);

        AICharacter[] npcs = FindObjectsOfType<AICharacter>(false);
        charList.makeItem = CharDropDown;
        charList.bindItem = (item, index) => { (item as DropdownField).label = npcs[index].name; };
        charList.itemsSource = npcs;
        charList.selectionChanged += OnCharacterSelectionChange;

        rootVisualElement.schedule.Execute(messageList.Rebuild).Every(100);
    }

    private DropdownField CharDropDown()
    {
        List<string> choices = new List<string> { "shortmem", "longmem" };
        var dropdown = new DropdownField(choices, 0);
        dropdown.RegisterValueChangedCallback((evt) => Debug.Log(evt.newValue));
        return dropdown;
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
                selected.BindListViewToCharacterShortMem(messageList);
            }

        }
    }

    void Update()
    {
    }

    public void Dumb()
    {
    }

    public void Test()
    {
        Debug.Log("IT WORKED!");
    }

}
