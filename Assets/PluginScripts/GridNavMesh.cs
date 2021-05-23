using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

/*public enum ShapeType
{
    Square,
    Hexagon
}*/

public class GridNavMesh : MonoBehaviour
{
    Vector2 gridArea;
    int cellSize;
    ShapeType cellShape;

    /*[MenuItem("Grid Navigation/Grid NavMesh")]
    static void Init()
    {
        GridNavMesh newMenu = (GridNavMesh)EditorWindow.GetWindow(typeof(GridNavMesh));
        newMenu.Show();
    }*/

    /*void OnGUI()
    {
        gridArea = EditorGUILayout.Vector2Field("Grid Size", gridArea);
        cellSize = EditorGUILayout.IntField("Cell Size", cellSize);
        cellShape = (ShapeType)EditorGUILayout.EnumPopup("Cell Shape", cellShape);

        if(GUILayout.Button("Create Grid"))
        {
            Generate();
        }
    }*/

    /*void Generate()
    {
        GameObject test = new GameObject("Grid");
        GridController newGrid = test.AddComponent(typeof(GridController)) as GridController;
        newGrid.Initialize(gridArea, cellSize, cellShape);
    }*/
}
