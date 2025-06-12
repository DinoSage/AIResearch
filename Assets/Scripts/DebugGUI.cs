using UnityEngine;

public class DebugGUI : MonoBehaviour
{
    [SerializeField] bool testDebug;
    // START ---> FPS calculation usefull variables
    public float updateInterval = 0.5f; // the time must pass to cache the frames value
    private double lastInterval; // last interval end time
    private float frames = 0; // frames over current interval
    private float fps; // current FPS
                       // <--- END
    private string textAreaContent = "";
    private Rect windowRect = new Rect(7f, 180f, 250f, 50f);
    private Rect buttonRect = new Rect(207f, 18f, 38f, 17f);
    private Vector2 scrollPosition; // the variable to control where the scrollview 'looks' into its child elements
                                    // you need to create the following style as you like to make possible to use them
    private GUIStyle debugTextArea;
    private GUIStyle yellowLabel;
    private GUIStyle redLabel;
    private GUIStyle greenLabel;
    private GUIStyle captionLabel;
    private GUIStyle lineLabel;
    // the flag to check if enable or not the features of your project
    private bool feature1Flag = true;
    private bool feature2Flag = true;

    public void Start()
    {
        // FPS variables initialization
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    } // Start

    public void Update()
    {
        calculateFPS();
        //updateDebugDataValues();
    } // Update

    public void OnGUI()
    {
        if (!testDebug)
        {
            // make a popup window
            windowRect = GUILayout.Window(0, windowRect, fillWindow, "DEBUG Window");
            // the window can be dragged around by the users - make sure that it doesn't go offscreen.
            windowRect.x = Mathf.Clamp(windowRect.x, 0.0f, Screen.width - windowRect.width);
            windowRect.y = Mathf.Clamp(windowRect.y, 0.0f, Screen.height - windowRect.height);
        }
    } // OnGUI

    private void fillWindow(int windowID)
    {
        // make the window be draggable only in the top 20 pixels.
        GUI.DragWindow(new Rect(0f, 0f, (float)System.Decimal.MaxValue, 20f));
        // the label for first window part
        GUILayout.Label("Marked Variables Status");
        // we are going to create the text area inside a scroll view to make sossible to scroll the content if necessary
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(120));
        GUILayout.TextArea(textAreaContent, (GUILayoutOption[])null);
        GUILayout.Button("TEST");
        GUILayout.EndScrollView();
        // the clear button
        // N.B. by pressing this button you will clear all ArrayList elements so
        //      you cannot no more see the  informations loaded in the current
        //      instantiated objects
        if (GUI.Button(buttonRect, "Clear"))
        {
            // delete the text area content
            textAreaContent = "";
            // delete all ArrayList items
            //DebugManager.clearItems();
        } // if
          // divider
        GUILayout.Label("", lineLabel);
        // the label for second window part
        GUILayout.Label("Others DEBUG features");
        // write the FPS value with 3 different colors depending by values
        if (fps > 30)
            GUILayout.Label("FPS: " + fps.ToString("f2"), greenLabel);
        else
           if (fps > 10)
            GUILayout.Label("FPS: " + fps.ToString("f2"), yellowLabel);
        else
            GUILayout.Label("FPS: " + fps.ToString("f2"), redLabel);
        // create the toggles button by arrange them horizontally
        GUILayout.BeginHorizontal();
        bool feature1FlagNew = GUILayout.Toggle(feature1Flag, "Feature1", (GUILayoutOption[])null);
        bool feature2FlagNew = GUILayout.Toggle(feature2Flag, "Feature2", (GUILayoutOption[])null);
        GUILayout.EndHorizontal();
    } // fillWindow

    private void calculateFPS()
    {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval)
        {
            fps = (float)(frames / (timeNow - lastInterval));
            frames = 0;
            lastInterval = timeNow;
        } // if
    } // calculateFPS
} // DebugGUI
