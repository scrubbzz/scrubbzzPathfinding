using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable
{
    //Positioning of each node.
    public float xPos;
    public float yPos;
    public int nodeCount;

    //For pathfinding.
    public float GCost;
    public float HCost;
    //public float fCost;

    public bool isTraversable { get; private set; }
    public bool wasVisited { get; set; }

    public float FCost //Constructor to get fCost. fCost will always be calculated the same way so no need to let the variable 
    {                  //be accessed and edited, hence the constructor.
        get
        {
            return  GCost + HCost;
        }
    }
   

    public Vector3 worldPos { get; private set; }
    public Vector3Int gridPos { get; private set; }//The postion of each node relative to node size, if size is 5 by 5, then 
                                                   //Then going 10 units along the x axis, you would reach only the second node.

    public Node parent;//This will be the current node, the parent of all its neighbours.
   public Node(Vector3 worldPos, Vector3Int gridPos, bool traversable)//Constructor to make node instances.
    {
        this.gridPos = gridPos;
        this.worldPos = worldPos;
        isTraversable = traversable;
    }

    public int CompareTo(object obj)//Specified IComparable implementation used by the 'sort' function in the A* class.
    {
        Node n = ((Node)obj);

        if (n.FCost < FCost)//Checking if obj FCost is less that this.FCost i.e the current node.
            return 1; //Moves obj node to the right of current node, to be in the index after it in the list.
        if (n.FCost > FCost)
            return -1; //Moves obj node to the left of the current node.

        return 0;
    }
}

