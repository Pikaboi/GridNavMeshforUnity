using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNavigator : MonoBehaviour
{
    public GridTraversal moveType;

    public GridController currentGrid;
    public float speed = 50;
    Rect CurrentCell;
    Rect Destination;
    Vector2Int cellID;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<GridController>() != null)
        {
            currentGrid = collision.gameObject.GetComponent<GridController>();
            GetCurrentCell();
        }
    }

    public void SetDestination(Rect _Destination) {
        Destination = _Destination;
    }

    private void GetCurrentCell()
    {
        for (int i = 0; i < currentGrid.gridArea.x; i++)
        {
            for(int j = 0; j < currentGrid.gridArea.y; j++)
            {
                if (currentGrid.cells[i,j].Contains(new Vector2(transform.position.x, transform.position.z)))
                {
                    CurrentCell = currentGrid.cells[i,j];
                    cellID = new Vector2Int(i, j);
                    break;
                }
            }
        }
    }

    public void Move()
    {
        if (CurrentCell != Destination)
        {
            switch (moveType)
            {
                case GridTraversal.Direction4:
                    Direction4Move();
                    break;
                case GridTraversal.Direction8:
                    Direction8Move();
                    break;
            }
        }
    }

    void Direction4Move()
    {
        GetCurrentCell();

        //Yooo A* Alogirithm woooo
        //g = parent.g + distance between node and parent
        //h = max {abs(currentx - destx), abs(currenty - desty)}
        //Get Successors
        List<Rect> successors = new List<Rect>();

        //check we can move left
        if (cellID.x - 1 >= 0)
        {
            successors.Add(currentGrid.cells[cellID.x - 1, cellID.y]);
        }

        //check we can move right
        if (cellID.x + 1 <= currentGrid.gridArea.x - 1)
        {
            successors.Add(currentGrid.cells[cellID.x + 1, cellID.y]);
        }

        //check we can move Up
        if (cellID.y + 1 <= currentGrid.gridArea.y - 1)
        {
            successors.Add(currentGrid.cells[cellID.x, cellID.y + 1]);
        }

        //check we can move down
        if (cellID.y - 1 >= 0)
        {
            successors.Add(currentGrid.cells[cellID.x, cellID.y - 1]);
        }

        //Get a maximum, infinity since it should work with any combo.
        float h = Mathf.Infinity;
        int bestSuccessor = 0;

        for (int i = 0; i < successors.Count; i++)
        {
            float manhattan = (Mathf.Abs(successors[i].x - Destination.x) + Mathf.Abs(successors[i].y - Destination.y));

            if (manhattan < h)
            {
                h = manhattan;
                bestSuccessor = i;
            }
        }

        /*CurrentCell = successors[bestSuccessor];

        Debug.Log(CurrentCell);
        Debug.Log(Destination);*/

        //Debug.Log(successors[bestSuccessor].center.y - currentGrid.cellSize / 2);

        Vector3 lookDir = new Vector3(successors[bestSuccessor].center.x, transform.position.y, successors[bestSuccessor].center.y);

        transform.LookAt(lookDir);

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    void Direction8Move()
    {
        GetCurrentCell();

        //Yooo A* Alogirithm woooo
        //g = parent.g + distance between node and parent
        //h = max {abs(currentx - destx), abs(currenty - desty)}
        //Get Successors
        List<Rect> successors = new List<Rect>();

        //check we can move left
        if (cellID.x - 1 >= 0)
        {
            successors.Add(currentGrid.cells[cellID.x - 1, cellID.y]);
        }

        //check we can move right
        if (cellID.x + 1 <= currentGrid.gridArea.x - 1)
        {
            successors.Add(currentGrid.cells[cellID.x + 1, cellID.y]);
        }

        //check we can move Up
        if (cellID.y + 1 <= currentGrid.gridArea.y - 1)
        {
            successors.Add(currentGrid.cells[cellID.x, cellID.y + 1]);
        }

        //check we can move down
        if (cellID.y - 1 >= 0)
        {
            successors.Add(currentGrid.cells[cellID.x, cellID.y - 1]);
        }

        //Check we can move Bottom-Left
        if (cellID.y - 1 >= 0 && cellID.x - 1 >= 0)
        {
            successors.Add(currentGrid.cells[cellID.x - 1, cellID.y - 1]);
        }

        //Check we can move Bottom-Right
        if (cellID.y - 1 >= 0 && cellID.x + 1 <= currentGrid.gridArea.x - 1)
        {
            successors.Add(currentGrid.cells[cellID.x + 1, cellID.y - 1]);
        }

        //Check we can move Top-Left
        if (cellID.y + 1 <= currentGrid.gridArea.y - 1 && cellID.x - 1 >= 0)
        {
            successors.Add(currentGrid.cells[cellID.x - 1, cellID.y + 1]);
        }

        //Check we can move Top-Right
        if (cellID.y + 1 <= currentGrid.gridArea.y - 1 && cellID.x + 1 <= currentGrid.gridArea.x - 1)
        {
            successors.Add(currentGrid.cells[cellID.x + 1, cellID.y + 1]);
        }

        //Get a maximum, infinity since it should work with any combo.
        float h = Mathf.Infinity;
        int bestSuccessor = 0;

        for (int i = 0; i < successors.Count; i++)
        {
            float dx = Mathf.Abs(successors[i].x - Destination.x);
            float dy = Mathf.Abs(successors[i].y - Destination.y);

            float Diagonal = (dx + dy) + (Mathf.Sqrt(2) - 2) * Mathf.Min(dx, dy);

            if (Diagonal < h)
            {
                h = Diagonal;
                bestSuccessor = i;
            }
        }

        Vector3 lookDir = new Vector3(successors[bestSuccessor].center.x, transform.position.y, successors[bestSuccessor].center.y);

        transform.LookAt(lookDir);

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    void MoveToDestCenter()
    {
        Vector3 lookDir = new Vector3(Destination.center.x, transform.position.y, Destination.center.y);

        transform.LookAt(lookDir);

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }
}
