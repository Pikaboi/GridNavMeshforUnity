using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Vector2 gridArea = new Vector2(10, 10);
    public int cellSize = 10;
    public ShapeType cellShape = ShapeType.Square;

    List<Rect> cells = new List<Rect>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Initialise the grid
    public void Initialize()
    {
        //This gets the size of its parent object
        float x = GetComponent<MeshFilter>().sharedMesh.bounds.max.x * transform.localScale.x;
        float z = GetComponent<MeshFilter>().sharedMesh.bounds.max.z * transform.localScale.z;

        //We clear cells from an old initialization
        cells.Clear();

        //Generate Each cell
        for(int i = 0; i < gridArea.x; i++)
        {
            for(int j = 0; j < gridArea.y; j++)
            {
                //Set the cells position and size
                cells.Add(new Rect(new Vector2(i * cellSize, j * cellSize) - new Vector2(x - cellSize / 2 , z - cellSize / 2), new Vector2(cellSize, cellSize)));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //Draw the Cells as Gizmos so it is visible to the user in editor
    private void OnDrawGizmos()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            Vector3 center = new Vector3(cells[i].x, 0.0f, cells[i].y);
            Vector3 size = new Vector3(cellSize, 1.0f, cellSize);
            Gizmos.DrawWireCube(transform.position + center, size);
        }
    }
}
