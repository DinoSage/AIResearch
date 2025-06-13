using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugEditor : EditorWindow
{
    private static EditorWindow instance;

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

        var charList = new ListView();
        splitView.Add(charList);

        var messageList = new ListView();
        splitView.Add(messageList);

        AICharacter[] npcs = FindObjectsOfType<AICharacter>(false);

        charList.makeItem = () => new Label();
        charList.bindItem = (item, index) => { (item as Label).text = npcs[index].name; };
        charList.itemsSource = npcs;
        charList.selectionChanged += OnCharacterSelectionChange;
    }

    private void OnCharacterSelectionChange(IEnumerable<object> selectedItems)
    {

    }

    void Update()
    {
        //label.schedule.Execute(Dumb).Every(1000);
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
