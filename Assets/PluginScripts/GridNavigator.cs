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
                break;
            }
        }
    }

    public void Move()
    {
        
    }

}
