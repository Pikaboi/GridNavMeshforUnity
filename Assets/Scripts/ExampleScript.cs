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
            GridNavigator.Move();
        }
    }

    void FixedUpdate()
    {
        //if mouse button (left hand side) pressed instantiate a raycast
        if (Input.GetMouseButton(0))
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //draw invisible ray cast/vector
                Debug.DrawLine(ray.origin, hit.point);
                //log hit area to the console

                if (GridNavigator.currentGrid != null) {
                    foreach (Rect r in GridNavigator.currentGrid.cells)
                    {
                        if (r.Contains(new Vector2(hit.point.x, hit.point.z)))
                        {
                            GridNavigator.SetDestination(r);
                        }
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Sets the default destination
        if(GridNavigator.currentGrid != null)
        {
            GridNavigator.SetDestination(GridNavigator.currentGrid.cells[(int)dest.x - 1, (int)dest.y - 1]);
        }
    }
}
