using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugEditor : EditorWindow
{
    private Label label;

    public static EditorWindow instance;

    public static void ShowDebugWindow()
    {
        if (instance == null)
        {
            EditorWindow wnd = GetWindow<DebugEditor>();
            wnd.titleContent = new GUIContent("Debug Window");
            instance = wnd;
        }
        // This method is called when the user selects the menu item in the Editor
    }

    public void CreateGUI()
    {
        label = new Label("Testing");
        rootVisualElement.Add(label);
        var btn = new Button(Test);
        var btnLabel = new Label("Click Me");
        btn.Add(btnLabel);
        rootVisualElement.Add(btn);
    }

    void Update()
    {
        //label.schedule.Execute(Dumb).Every(1000);
    }

    public void Dumb()
    {
        label.text += "!";
    }

    public void Test()
    {
        Debug.Log("IT WORKED!");
    }

}
