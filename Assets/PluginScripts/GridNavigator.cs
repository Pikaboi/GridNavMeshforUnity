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
    Rect DestCenter;
    Vector2Int cellID;
    List<GridNode> closedList = new List<GridNode>();
    List<GridNode> openList = new List<GridNode>();
    GridNode[,] nodes;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<GridController>() != null)
        {
            //Get the grid and current cell on contact
            currentGrid = collision.gameObject.GetComponent<GridController>();
            GetCurrentCell();
        }
    }

    public void SetDestination(Rect _Destination) {
        //We clear the closedList so it doesnt get stuck on places its been
        closedList.Clear();
        Destination = _Destination;
        //Get the center of the destination
        DestCenter = new Rect(Destination.center, Destination.size);
        Debug.Log(closedList.Count);
    }

    public void SetDestination(int _gridX, int _gridY)
    {
        //We clear the closedList so it doesnt get stuck on places its been
        closedList.Clear();
        Destination = currentGrid.cells[_gridX, _gridY];
        //Get the center of the destination
        DestCenter = new Rect(Destination.center, Destination.size / 2);
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

    private void GetCurrentNode(GridNode _current)
    {
        for (int i = 0; i < currentGrid.gridArea.x; i++)
        {
            for (int j = 0; j < currentGrid.gridArea.y; j++)
            {
                if (currentGrid.cells[i, j] == _current.m_cell)
                {
                    cellID = new Vector2Int(i, j);
                    break;
                }
            }
        }
    }

    public void Move()
    {
        if(closedList.Count == 0)
        {
            GetPath();
        }
        //Move with A* Algorithm
        /*if (CurrentCell != Destination)
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
        } //Move to center now that it has arrived 
        else if (!DestCenter.Contains(transform.position))
        {
            MoveToDestCenter();
        }*/
    }

    public List<GridNode> GetPath()
    {
        GridNode Start = new GridNode(CurrentCell);

        openList = new List<GridNode>();
        openList.Add(Start);
        closedList = new List<GridNode>();

        Start.m_g = 0;
        Start.m_h = GetDistance(Start.m_cell);

        //Main loop
        //Set current node
        while (openList.Count > 0)
        {
            GridNode CurrentNode = GetLowestCost();

            //Check if reached the end
            if (CurrentNode.m_cell == Destination)
            {
                //return the path
            }

            openList.Remove(CurrentNode);
            closedList.Add(CurrentNode);

            List<GridNode> successors = new List<GridNode>();
            switch (moveType)
            {
                case GridTraversal.Direction4:
                    successors = Direction4Move(CurrentNode);
                    break;
                case GridTraversal.Direction8:
                    successors = Direction8Move(CurrentNode);
                    break;
            }
        }



        //Remove node from open list
        //Add it too closed

        //loop through successors
        //Ignore if in closed
        //calculate the distance
        //if distance smaller, set as successor
        //Update Values
        //if not on open list, add it

        //return nothing if no path
    }

    GridNode GetLowestCost()
    {
        GridNode lowestCost = openList[0];

        for(int i = 0; i < openList.Count; i++)
        {
            if(openList[i].m_f < lowestCost.m_f)
            {
                lowestCost = openList[i];
            }
        }

        return lowestCost;
    }

    float GetDistance(Rect start)
    {
        float dis = 0;
        switch (moveType)
        {
            case GridTraversal.Direction4:
                //Manhattan
                dis = (Mathf.Abs(start.x - Destination.x) + Mathf.Abs(start.y - Destination.y)) + currentGrid.GetCost(start);
                break;
            case GridTraversal.Direction8:
                //Diagonal
                float dx = Mathf.Abs(start.x - Destination.x);
                float dy = Mathf.Abs(start.y - Destination.y);

                dis = (dx + dy) + (Mathf.Sqrt(2) - 2) * Mathf.Min(dx, dy) + currentGrid.GetCost(start);
                break;
        }

        return dis;
    }

    List<GridNode> Direction4Move(GridNode _current)
    {
        GetCurrentNode(_current);
        //Get Successors
        List<GridNode> successors = new List<GridNode>();

        //check we can move left
        if (cellID.x - 1 >= 0)
        {
            bool obs = currentGrid.FindObstacle(cellID.x - 1, cellID.y, this);
            if (!obs)
            {
                successors.Add(currentGrid.nodes[cellID.x - 1, cellID.y]);
            }
        }

        //check we can move right
        if (cellID.x + 1 <= currentGrid.gridArea.x - 1)
        {
            bool obs = currentGrid.FindObstacle(cellID.x + 1, cellID.y, this);
            if (!obs)
            {
                successors.Add(currentGrid.nodes[cellID.x + 1, cellID.y]);
            }
        }

        //check we can move Up
        if (cellID.y + 1 <= currentGrid.gridArea.y - 1)
        {
            bool obs = currentGrid.FindObstacle(cellID.x, cellID.y + 1, this);
            if (!obs)
            {
                successors.Add(currentGrid.nodes[cellID.x, cellID.y + 1]);
            }
        }

        //check we can move down
        if (cellID.y - 1 >= 0)
        {
            bool obs = currentGrid.FindObstacle(cellID.x, cellID.y - 1, this);
            if (!obs)
            {
                successors.Add(currentGrid.nodes[cellID.x, cellID.y - 1]);
            }
        }

        return successors;
    }

    List<GridNode> Direction8Move(GridNode _current)
    {
        GetCurrentNode(_current);
        //Get SUccessors
        List<GridNode> successors = new List<GridNode>();

        //check we can move left
        if (cellID.x - 1 >= 0)
        {
            bool obs = currentGrid.FindObstacle(cellID.x - 1, cellID.y, this);
            if (!obs)
            {
                successors.Add(currentGrid.nodes[cellID.x - 1, cellID.y]);
            }
        }

        //check we can move right
        if (cellID.x + 1 <= currentGrid.gridArea.x - 1)
        {
            bool obs = currentGrid.FindObstacle(cellID.x + 1, cellID.y, this);
            if (!obs)
            {
                successors.Add(currentGrid.nodes[cellID.x + 1, cellID.y]);
            }
        }

        //check we can move Up
        if (cellID.y + 1 <= currentGrid.gridArea.y - 1)
        {
            bool obs = currentGrid.FindObstacle(cellID.x, cellID.y + 1, this);
            if (!obs)
            {
                successors.Add(currentGrid.nodes[cellID.x, cellID.y + 1]);
            }
        }

        //check we can move down
        if (cellID.y - 1 >= 0)
        {
            bool obs = currentGrid.FindObstacle(cellID.x, cellID.y - 1, this);
            if (!obs)
            {
                successors.Add(currentGrid.nodes[cellID.x, cellID.y - 1]);
            }
        }

        //Check we can move Bottom-Left
        if (cellID.y - 1 >= 0 && cellID.x - 1 >= 0)
        {
            //We do 3 checks so it doesnt try to cut through obstacles
            bool obs = currentGrid.FindObstacle(cellID.x - 1, cellID.y - 1, this);
            bool obs2 = currentGrid.FindObstacle(cellID.x - 1, cellID.y, this);
            bool obs3 = currentGrid.FindObstacle(cellID.x, cellID.y - 1, this);
            if (!obs && !obs2 && !obs3)
            {
                successors.Add(currentGrid.nodes[cellID.x - 1, cellID.y - 1]);
            }
        }

        //Check we can move Bottom-Right
        if (cellID.y - 1 >= 0 && cellID.x + 1 <= currentGrid.gridArea.x - 1)
        {
            //We do 3 checks so it doesnt try to cut through obstacles
            bool obs = currentGrid.FindObstacle(cellID.x + 1, cellID.y - 1, this);
            bool obs2 = currentGrid.FindObstacle(cellID.x + 1, cellID.y, this);
            bool obs3 = currentGrid.FindObstacle(cellID.x, cellID.y - 1, this);
            if (!obs && !obs2 && !obs3)
            {
                successors.Add(currentGrid.nodes[cellID.x + 1, cellID.y - 1]);
            }
        }

        //Check we can move Top-Left
        if (cellID.y + 1 <= currentGrid.gridArea.y - 1 && cellID.x - 1 >= 0)
        {
            //We do 3 checks so it doesnt try to cut through obstacles
            bool obs = currentGrid.FindObstacle(cellID.x - 1, cellID.y + 1, this);
            bool obs2 = currentGrid.FindObstacle(cellID.x - 1, cellID.y, this);
            bool obs3 = currentGrid.FindObstacle(cellID.x, cellID.y + 1, this);
            if (!obs && !obs2 && !obs3)
            {
                successors.Add(currentGrid.nodes[cellID.x - 1, cellID.y + 1]);
            }
        }

        //Check we can move Top-Right
        if (cellID.y + 1 <= currentGrid.gridArea.y - 1 && cellID.x + 1 <= currentGrid.gridArea.x - 1)
        {
            //We do 3 checks so it doesnt try to cut through obstacles
            bool obs = currentGrid.FindObstacle(cellID.x + 1, cellID.y + 1, this);
            bool obs2 = currentGrid.FindObstacle(cellID.x + 1, cellID.y, this);
            bool obs3 = currentGrid.FindObstacle(cellID.x, cellID.y + 1, this);
            if (!obs && !obs2 && !obs3)
            {
                successors.Add(currentGrid.nodes[cellID.x + 1, cellID.y + 1]);
            }
        }

        return successors;
    }

    bool isDest(Rect rect)
    {
        if (rect == Destination)
        {
            return true;
        }
        return false;
    }


    //Moves the player closer to the center of its destination before stopping
    void MoveToDestCenter()
    {
        Vector3 lookDir = new Vector3(Destination.center.x, transform.position.y, Destination.center.y);

        transform.LookAt(lookDir);

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    private void OnDrawGizmos()
    {
        if (currentGrid != null)
        {
            foreach (Rect r in closedList)
            {
                Gizmos.color = Color.red;
                    Vector3 center = new Vector3(r.center.x, 0.0f, r.center.y);
                    Vector3 size = new Vector3(100.0f, 1.0f, 100.0f);
                    Gizmos.DrawWireCube(center, size);
            }
        }
    }
}
