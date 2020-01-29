using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BehaviourTab))]
public class TabScript : Editor
{
    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
        Color myStyleColor = Color.red;
        myFoldoutStyle.fontStyle = FontStyle.Bold;
        myFoldoutStyle.normal.textColor = myStyleColor;
        myFoldoutStyle.onNormal.textColor = myStyleColor;
        myFoldoutStyle.hover.textColor = myStyleColor;
        myFoldoutStyle.onHover.textColor = myStyleColor;
        myFoldoutStyle.focused.textColor = myStyleColor;
        myFoldoutStyle.onFocused.textColor = myStyleColor;
        myFoldoutStyle.active.textColor = myStyleColor;
        myFoldoutStyle.onActive.textColor = myStyleColor;
        DrawDefaultInspector();
    }
}
