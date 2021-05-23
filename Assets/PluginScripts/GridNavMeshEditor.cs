using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ShapeType
{
    Square,
    Hexagon
}

[CustomEditor(typeof(GridController))]
[CanEditMultipleObjects]
public class GridNavMeshEditor : Editor
{
    SerializedProperty gridAreaProp;
    SerializedProperty cellSizeProp;
    SerializedProperty cellShapeProp;

    private void OnEnable()
    {
        //Get the objects variables
        gridAreaProp = serializedObject.FindProperty("gridArea");
        cellSizeProp = serializedObject.FindProperty("cellSize");
        cellShapeProp = serializedObject.FindProperty("cellShape");
    }

    public override void OnInspectorGUI()
    {
        //I update the objects variables to the inspectors value
        serializedObject.Update();
        EditorGUILayout.PropertyField(gridAreaProp, new GUIContent("Grid Area"));
        EditorGUILayout.PropertyField(cellSizeProp, new GUIContent("Cell Size"));
        EditorGUILayout.PropertyField(cellShapeProp, new GUIContent("Cell Shape"));
        serializedObject.ApplyModifiedProperties();

        //Grab the object we are editing
        GridController cont = (GridController)target;
        //Initialize the grid
        cont.Initialize(cont.gridArea, cont.cellSize, cont.cellShape);
    }
}
