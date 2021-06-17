using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    public Rect m_cell;

    public float m_g;
    public float m_h;
    public float m_f;

    public GridNode prevNode;

    public GridNode(Rect _cell)
    {
        this.m_cell = _cell;
    }

    public void GetF()
    {
        m_f = m_g + m_h;
    }
}
