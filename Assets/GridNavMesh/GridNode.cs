using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    //This contains the nodes for the A* algorithm
    public Rect m_cell;

    public float m_g;
    public float m_h;
    public float m_f;

    public GridNode prevNode;

    //initialises the object
    public GridNode(Rect _cell)
    {
        this.m_cell = _cell;
    }

    //Gets the F cost
    public void GetF()
    {
        m_f = m_g + m_h;
    }
}
