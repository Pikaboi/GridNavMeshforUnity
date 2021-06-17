using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    /* TEST SCRIPT
     * NOT PART OF PLUGIN
     */

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
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(GridNavigator.currentGrid != null)
        {
            GridNavigator.SetDestination(GridNavigator.currentGrid.cells[(int)dest.x - 1, (int)dest.y - 1]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Respawn")
        {
            GridNavigator.SetDestination(GridNavigator.currentGrid.cells[0, 0]);
            dest = new Vector2(1, 1);
        }
    }
}
