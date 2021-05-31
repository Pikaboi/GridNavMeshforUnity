using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum GridTraversal
{
    Direction4,
    Direction8
};

[CustomEditor(typeof(GridNavigator))]
[CanEditMultipleObjects]
public class GridNavigatorEditor : Editor
{
    SerializedProperty MovementTypeProp;
    SerializedProperty SpeedProp;

    private void OnEnable()
    {
        MovementTypeProp = serializedObject.FindProperty("moveType");
        SpeedProp = serializedObject.FindProperty("speed");
    }

    public override void OnInspectorGUI()
    {
        //Update objects variables to inspectors value
        serializedObject.Update();
        EditorGUILayout.PropertyField(MovementTypeProp, new GUIContent("Movement Type"));
        EditorGUILayout.PropertyField(SpeedProp, new GUIContent("Agent Speed"));
        serializedObject.ApplyModifiedProperties();
    }
}
