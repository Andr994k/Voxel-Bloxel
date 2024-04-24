using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;

    public Node parent;

    int heapindex;

    // Definerer hvad en node skal indeholde
    public Node(bool _walkable, Vector3 _worldpos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldpos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost 
    {  
        get 
        { 
            return gCost + hCost; 
        } 
    }

    public int HeapIndex
    {
        get
        {
            return heapindex;
        }
        set
        {
            heapindex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) 
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

}
