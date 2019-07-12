using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProjectView : EditorWindow {


    private UnityFolder Assets;
    private Vector2 Scroll;


    [MenuItem("Window/CustomProjectView")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ProjectView window = (ProjectView)EditorWindow.GetWindow(typeof(ProjectView));
        window.Show();
    }


    void OnGUI()
    {
        Scroll=GUILayout.BeginScrollView(Scroll);
        GUILayout.BeginVertical();


        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Fetch", EditorStyles.toolbarButton))
            Assets = new UnityFolder("Assets",null,0,position);
        if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
            Assets = null;
        GUILayout.EndHorizontal();

        if (Assets != null)
            Assets.VisualizeFolder();

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

    }


}
