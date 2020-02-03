using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TreeVisualization : EditorWindow
{
    //Current Selected character nodes
    public List<string> CurrentEnemyTree = new List<string>();
    public GameObject myObject;
    public BehaviourTree currentTree;

    private CNode mNode;
    public GUIStyle myGUIStyle;
    public GUIStyle DefaultStyle;

    [MenuItem("Window/Tree Visualization Tab")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<TreeVisualization>("Tree Visualization");
    }

    public static void AddToList()
    {
        //CurrentEnemyTree.Add("hiii there");
    }

    //CustomList t;
    //SerializedObject GetTarget;
    //SerializedProperty ThisList;
    //int ListSize;


    //void OnEnable()
    //{
    //    t = (CustomList)target;
    //    GetTarget = new SerializedObject(t);
    //    ThisList = GetTarget.FindProperty("MyList"); // Find the List in our script and create a refrence of it
    //}

    public static string GetCurrentEnemyState(ENodeState state)
    {
        switch (state)
        {
            case ENodeState.Success:
                return "Success";
            case ENodeState.Failure:
                return "Failure";
            case ENodeState.Running:
                return "Running";
        }
        return "Cannot Find a State ";
    }

    public void OnInspectorUpdate()
    {
        if (EditorApplication.isPlaying)
        {
            // This will only get called 10 times per second.
            Repaint();
        }
    }

    // WHat is show on the Window
    private void OnGUI()
    {
        GUILayout.Label("Enemy Name: ");
        //EditorGUILayout..
        // myGUIStyle.p
        //myGUIStyle.normal.textColor = Color.yellow;

        CurrentEnemyTree.Add("hiii there");
        //GUILayout.Label(CurrentEnemyTree[0]);
        myObject = (GameObject)EditorGUILayout.ObjectField("CurrentPrefab:", myObject, typeof(GameObject), true);
        currentTree = myObject.GetComponent<BehaviourTree>();
        GUILayout.Label("Enemy Name: " + myObject.name);

        switch (currentTree.currentState)
        {
            case ENodeState.Success:
                myGUIStyle.normal.textColor = Color.green;
                break;
            case ENodeState.Failure:
                myGUIStyle.normal.textColor = Color.red;
                break;
            case ENodeState.Running:
                myGUIStyle.normal.textColor = Color.yellow;
                break;
        }

        if (EditorApplication.isPlaying)
        {
            GUI.Label(new Rect(7, 62, 200, 60), "Enemy State: ", DefaultStyle);
            GUI.Label(new Rect(83, 62, 200, 60), GetCurrentEnemyState(currentTree.currentState), myGUIStyle);
            GUI.Label(new Rect(7, 90, 200, 60), "Current Node: " + currentTree.currentnode.GetName(), DefaultStyle);
            //GUI.Label(new Rect(83, 62, 200, 60), "Enemy State: ");

        }
    }



}
