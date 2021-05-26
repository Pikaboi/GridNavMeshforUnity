using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNavigator : MonoBehaviour
{
    GridController currentGrid;
    float speed;
    Rect CurrentCell;
    Rect Destination;
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
        Destination = currentGrid.cells[0,0];
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
                    Debug.Log(new Vector2(i, j));
                    //Move();
                    break;
                }
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
        
    }
}
