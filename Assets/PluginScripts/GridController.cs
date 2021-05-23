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
    public void Initialize(Vector2 _Area, int _Size, ShapeType _Shape)
    {
        cells.Clear();
        gridArea = _Area;
        cellSize = _Size;
        cellShape = _Shape;

        for(int i = 0; i < gridArea.x; i++)
        {
            for(int j = 0; j < gridArea.y; j++)
            {
                cells.Add(new Rect(new Vector2(i * cellSize, j * cellSize), new Vector2(cellSize, cellSize)));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

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
