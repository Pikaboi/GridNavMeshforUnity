using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2 gridArea = new Vector2(10, 10);
    public int cellSize = 10;
    public ShapeType cellShape = ShapeType.Square;

    public Rect[,] cells = new Rect[10,10];
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    //Initialise the grid
    public void Initialize()
    {
        //This gets the size of its parent object
        float x = GetComponent<MeshFilter>().sharedMesh.bounds.max.x * transform.localScale.x;
        float z = GetComponent<MeshFilter>().sharedMesh.bounds.max.z * transform.localScale.z;

        //We clear cells from an old initialization
        cells = new Rect[(int)gridArea.x, (int)gridArea.y];
        
        //Generate Each cell
        for(int i = 0; i < gridArea.x; i++)
        {
            for(int j = 0; j < gridArea.y; j++)
            {
                //Set the cells position and size
                //cells.Add(new Rect(new Vector2(i * cellSize, j * cellSize) - new Vector2(x - cellSize / 2 , z - cellSize / 2), new Vector2(cellSize, cellSize)));
                cells[i, j] = new Rect(new Vector2(i * cellSize, j * cellSize) - new Vector2(x, z), new Vector2(cellSize, cellSize));
            }
        }
    }

    public bool FindObstacle(int _x, int _y, GridNavigator _Navigator)
    {
        Vector3 Overlap = new Vector3(cells[_x, _y].center.x, transform.position.y, cells[_x, _y].center.y);
        Vector3 OverlapSize = new Vector3(cellSize / 2, cellSize / 2, cellSize / 2);
        Collider[] colliders = Physics.OverlapBox(Overlap, OverlapSize, Quaternion.identity);

        foreach(Collider co in colliders)
        {
            if(co.GetComponent<GridNavObstacle>() != null)
            {
                return true;
            }

            if(co.GetComponent<GridNavigator>() != null && co.GetComponent<GridNavigator>() != _Navigator)
            {
                return true;
            }
        }

        return false;
    }

    //Draw the Cells as Gizmos so it is visible to the user in editor
    private void OnDrawGizmos()
    {
        for (int i = 0; i < gridArea.x; i++)
        {
            for (int j = 0; j < gridArea.y; j++)
            {
                Vector3 center = new Vector3(cells[i, j].center.x, 0.0f, cells[i, j].center.y);
                Vector3 size = new Vector3(cellSize, 1.0f, cellSize);
                Gizmos.DrawWireCube(transform.position + center, size);
            }
        }
    }

}
