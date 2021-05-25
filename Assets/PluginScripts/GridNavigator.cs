using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNavigator : MonoBehaviour
{
    GridController currentGrid;
    float speed;
    Rect CurrentCell;
    Rect Destination;
    int cellID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentCell();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<GridController>() != null)
        {
            currentGrid = collision.gameObject.GetComponent<GridController>();
            GetCurrentCell();
        }
    }

    public void SetDestination(Rect _Destination) {
        Destination = currentGrid.cells[0];
    }

    public void GetCurrentCell()
    {
        for (int i = 0; i < currentGrid.cells.Count; i++)
        {
            if (currentGrid.cells[i].Contains(new Vector2(transform.position.x, transform.position.z)))
            {
                CurrentCell = currentGrid.cells[i];
                cellID = i;
                //Debug.Log(cellID);
                Move();
                break;
            }
        }
    }

    public void Move()
    {
        //Yooo A* Alogirithm woooo
        //g = parent.g + distance between node and parent
        //h = max {abs(currentx - destx), abs(currenty - desty)}
        //Get Successors
        List<Rect> successors = new List<Rect>();

        //check we can move right
        if(cellID + 1 < currentGrid.cells.Count && cellID % currentGrid.gridArea.x != (currentGrid.gridArea.x - 1))
        {
            successors.Add(currentGrid.cells[cellID + 1]);
        }

        //Check we can move left
        if(cellID - 1 >= 0 && cellID % currentGrid.gridArea.x != 0)
        {
            successors.Add(currentGrid.cells[cellID - 1]);
        }

        //Check we can move Up
        if(cellID - 10 >= 0 && cellID % currentGrid.gridArea.y != 0)
        {
            successors.Add(currentGrid.cells[cellID - 10]);
        }

        //Check we can move Down
        if(cellID + 10 < currentGrid.cells.Count %% cellID % currentGrid.gridArea.y != (currentGrid.gridArea.y - 1))
        {
            successors.Add(currentGrid.cells[cellID + 10]);
        }

        Debug.Log(cellID);
        Debug.Log(successors.Count);
    }
}
