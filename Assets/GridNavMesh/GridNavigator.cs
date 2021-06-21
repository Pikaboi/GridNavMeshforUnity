using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNavigator : MonoBehaviour
{
    public GridTraversal moveType;

    public GridController currentGrid;
    public float speed = 50;
    public Rect CurrentCell;
    public Rect Destination;
    Rect DestCenter;
    Vector2Int cellID;
    List<GridNode> closedList = new List<GridNode>();
    List<GridNode> openList = new List<GridNode>();
    public List<GridNode> Path = new List<GridNode>();

    int pathNum = 1;

    //Sets the grid if they collide
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<GridController>() != null)
        {
            //Get the grid and current cell on contact
            currentGrid = collision.gameObject.GetComponent<GridController>();
            GetCurrentCell();
        }
    }

    //Set the destination
    public void SetDestination(Rect _Destination) {
        bool check = GridCheck(_Destination);
        if (!check)
        {
            return;
        }
        currentGrid.ResetNodes();
        GetCurrentCell();
        //We clear the closedList so it doesnt get stuck on places its been
        Destination = _Destination;
        //Get the center of the destination
        DestCenter = new Rect(Destination.center - Destination.size / 4, Destination.size / 2);
        Path = GetPath();
        pathNum = 1;
    }

    //Check the inputted Rect is on our grid
    bool GridCheck(Rect _checker)
    {
        for (int i = 0; i < currentGrid.gridArea.x; i++)
        {
            for (int j = 0; j < currentGrid.gridArea.y; j++)
            {
                if (_checker == currentGrid.cells[i,j])
                {
                    return true;
                }
            }
        }
        return false;
    }

    //Set the destination using co-ordinates
    public void SetDestination(int _gridX, int _gridY)
    {
        currentGrid.ResetNodes();
        GetCurrentCell();
        //We clear the closedList so it doesnt get stuck on places its been
        Destination = currentGrid.cells[_gridX, _gridY];
        //Get the center of the destination
        DestCenter = new Rect(Destination.center - Destination.size / 4, Destination.size / 2);
        Path = GetPath();
        pathNum = 1;
    }

    //Get the current cell the agent is on
    public void GetCurrentCell()
    {
        for (int i = 0; i < currentGrid.gridArea.x; i++)
        {
            for(int j = 0; j < currentGrid.gridArea.y; j++)
            {
                if (currentGrid.cells[i,j].Contains(new Vector2(transform.position.x, transform.position.z)))
                {
                    CurrentCell = currentGrid.cells[i,j];
                    break;
                }
            }
        }
    }

    //Get the node of the cell the agent is on.
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

    //Move the agent
    public void Move()
    {
        //Move if there is a path to follow
        if (Path != null && pathNum < Path.Count - 1)
        {
            //Look towards the path
            Vector3 lookDir = new Vector3(Path[pathNum].m_cell.center.x, transform.position.y, Path[pathNum].m_cell.center.y);
            gameObject.transform.LookAt(lookDir);
            //Move towards the path
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        } else if (Path != null && pathNum == Path.Count - 1 && !DestCenter.Contains(new Vector2(transform.position.x, transform.position.z)))
        {
            //Move it too center when its in the destination
            MoveToDestCenter();
        }

        if (Path != null) {
            //Check if the Navigator reached the next cell
            if (Path[pathNum].m_cell.Contains(new Vector2(transform.position.x, transform.position.z)))
            {
                //Check it doesnt go out of bounds
                if (pathNum != Path.Count - 1)
                {
                    pathNum++;
                }
            }
        }
    }

    //Get the path with A* algorithm
    private List<GridNode> GetPath()
    {
        GetCurrentCell();
        GridNode Start = new GridNode(CurrentCell);

        openList = new List<GridNode>();
        openList.Add(Start);
        closedList = new List<GridNode>();

        Start.m_g = 0;
        Start.m_h = GetDistance(Start.m_cell, Destination);

        //Main loop
        //Set current node
        while (openList.Count > 0)
        {
            GridNode CurrentNode = GetLowestCost();

            //Check if reached the end
            if (CurrentNode.m_cell == Destination)
            {
                //return the path
                return GeneratePath(CurrentNode);
            }

            //Remove node from open list
            openList.Remove(CurrentNode);
            //Add it too closed
            closedList.Add(CurrentNode);

            //Get the successors based on move type
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

            //loop through successors
            foreach (GridNode _node in successors)
            {
                //Ignore if in closed
                if (closedList.Contains(_node)) continue;

                //calculate the distance
                //if distance smaller, set as successor
                float g = CurrentNode.m_g + GetDistance(CurrentNode.m_cell, _node.m_cell);

                if(g < _node.m_g)
                {
                    //Update Values
                    _node.prevNode = CurrentNode;
                    _node.m_g = g;
                    _node.m_h = GetDistance(_node.m_cell, Destination);
                    _node.GetF();

                    //if not on open list, add it
                    if (!openList.Contains(_node))
                    {
                        openList.Add(_node);
                    }
                }
            }
        }
        //return nothing if no path
        return null;
    }

    //Generates the path from the end node
    List<GridNode> GeneratePath(GridNode _end)
    {
        //Backtrack through nodes to get the path
        List<GridNode> path = new List<GridNode>();
        path.Add(_end);
        GridNode curr = _end;
        while(curr.prevNode != null)
        {
            path.Add(curr.prevNode);
            curr = curr.prevNode;
        }

        path.Reverse();

        return path;
    }

    //Finds the lowest cost to add to the open list
    GridNode GetLowestCost()
    {
        GridNode lowestCost = openList[0];

        //Get the lowest F Cost
        for(int i = 0; i < openList.Count; i++)
        {
            if(openList[i].m_f < lowestCost.m_f)
            {
                lowestCost = openList[i];
            }
        }

        return lowestCost;
    }

    //Get the distance heuristic, based on the movement type
    float GetDistance(Rect start, Rect end)
    {
        float dis = 0;
        switch (moveType)
        {
            case GridTraversal.Direction4:
                //Manhattan
                dis = (Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y)) + currentGrid.GetCost(start);
                break;
            case GridTraversal.Direction8:
                //Diagonal
                float dx = Mathf.Abs(start.x - end.x);
                float dy = Mathf.Abs(start.y - end.y);

                dis = (dx + dy) + (Mathf.Sqrt(2) - 2) * Mathf.Min(dx, dy) + currentGrid.GetCost(start);
                break;
        }

        return dis;
    }

    //Get Successors in 4 directions
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

    //Get successors in 8 directions
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

    //Check if at destination
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
        //Look towards center and move
        Vector3 lookDir = new Vector3(Destination.center.x, transform.position.y, Destination.center.y);

        transform.LookAt(lookDir);

        transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }

    private void OnDrawGizmos()
    {
        if (currentGrid != null && Path != null)
        {
            //Draws the path of the Navigator
            foreach (GridNode r in Path)
            {
                //Notify the goal in green
                if (r != Path[Path.Count - 1])
                {
                    Gizmos.color = Color.red;
                } else
                {
                    Gizmos.color = Color.green;
                }
                Vector3 center = new Vector3(r.m_cell.center.x, currentGrid.gameObject.transform.position.y + 1.0f, r.m_cell.center.y);
                Vector3 size = new Vector3(currentGrid.cellSize, currentGrid.gameObject.transform.position.y + 1.0f, currentGrid.cellSize);
                Gizmos.DrawWireCube(center, size);
            }

            //The destinations center
            Gizmos.color = Color.blue;

            Vector3 center2 = new Vector3(DestCenter.center.x, currentGrid.gameObject.transform.position.y + 1.0f, DestCenter.center.y);
            Vector3 size2 = new Vector3(currentGrid.cellSize / 2, currentGrid.gameObject.transform.position.y + 1.0f, currentGrid.cellSize / 2);
            Gizmos.DrawWireCube(center2, size2);
        }
    }
}
