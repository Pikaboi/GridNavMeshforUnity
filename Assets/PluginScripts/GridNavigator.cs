using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNavigator : MonoBehaviour
{
    GridController currentGrid;
    float speed;
    Rect CurrentCell;
    Rect Destination;
    Vector2Int cellID;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<GridController>() != null)
        {
            currentGrid = collision.gameObject.GetComponent<GridController>();
            GetCurrentCell();
            SetDestination(currentGrid.cells[9, 9]);
        }
    }

    public void SetDestination(Rect _Destination) {
        Destination = _Destination;
    }

    public void GetCurrentCell()
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

            Vector3 lookDir = new Vector3(successors[bestSuccessor].center.x - currentGrid.cellSize / 2, transform.position.y, successors[bestSuccessor].center.y - currentGrid.cellSize / 2);

            transform.LookAt(lookDir);

            transform.Translate(transform.forward * 50 * Time.deltaTime, Space.World);
        }
    }
}
