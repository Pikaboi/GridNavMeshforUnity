using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    public GridNavigator GridNavigator;
    public Vector2 dest;
    // Start is called before the first frame update
    void Start()
    {
        GridNavigator = gameObject.GetComponent<GridNavigator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GridNavigator.currentGrid != null)
        {
            GridNavigator.Move();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(GridNavigator.currentGrid != null)
        {
            Debug.Log(GridNavigator.currentGrid.cells.Length);
            GridNavigator.SetDestination(GridNavigator.currentGrid.cells[(int)dest.x - 1, (int)dest.y - 1]);
        }
    }
}
