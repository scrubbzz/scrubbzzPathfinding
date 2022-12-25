using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class AStar 
{
    public Grid grid;

    /*[SerializeField] Vector3Int startNodeGrid;
    [SerializeField] Vector3Int endNodeGrid;*/
    public bool working = false;
    public Node startNode;
    public Node endNode;//The goal node.
    public Node currentNode;
    List<Node> neighbours = new List<Node>();//All of the neignbours in the immediate vicinity of the current node.

    List<Node> openList = new List<Node>();//Nodes still to be checked.
    List<Node> newImprovedClosedList = new List<Node>();//Nodes no longer needed to be checked.
    public List<Node> finalPath = new List<Node>();
    
    
    public void FindPath(Vector3Int startNodeGrid, Vector3Int endNodeGrid)
    {
        //Debug.Log("THE THREAD WORKS BRO");
        working = true;//This function should only be called if a path has not already been found.
         FindPathPrivate(startNodeGrid, endNodeGrid);
        working = false;
    }

    private bool FindPathPrivate(Vector3Int startNodeGrid, Vector3Int endNodeGrid)
    {
        //grid = new LineDrawer();
        if(!Initialize(startNodeGrid, endNodeGrid)) return false;
       


        while (openList.Count > 0)
        {
            //neighbours.Clear();
            openList.Sort();
            currentNode = openList[0];
            //currentNode.wasVisited = true;//Each node has a bool check that is set as true so that we know we visited it for some data resetting later.
            openList.RemoveAt(0);
            currentNode.wasVisited = true;//Each node has a bool check that is set as true so that we know we visited it for some data resetting later.
            newImprovedClosedList.Add(currentNode);//Add function adds stuff to the end.
            //currentNode = closedList[0];

            if (DidReachTarget())
            {
                GetPath(endNode);//Gets the parent of each previously visited node and adds them to the 'finalPath' list.
                finalPath.Reverse();//To get the nodes in the correct order to actually travel on.
                return true;
            }
            GetNeighbours(currentNode);//Call this once rather than in the loop below so you don't get a million neighbours...
            for (int i = 0; i < neighbours.Count; i++)//UNDERSTAND THIS PART YOU FOOL.
            {
                if (!neighbours[i].wasVisited)//If you can move on the node and it has yet to be checked.
                {
                    if (!openList.Contains(neighbours[i]))
                    {
                        int newMovementCost = CalculateDistance(neighbours[i].gridPos, startNode.gridPos); //This is the distance of an alternate path the neighbour could take to the start node,
                                                                                                           //moving in a straight line rather than through its parent to the start node.
                        CheckAltPath(i, newMovementCost);

                    }
                }
            }


        }
        //grid = null;
        return false;
    }
    /// <summary>
    /// Is alternate path shorter than neighbour's current path?
    /// </summary>
    /// <param name="i"></param>
    /// <param name="newMovementCost"></param>
    private void CheckAltPath(int i, int newMovementCost)
    {
        if (newMovementCost < neighbours[i].GCost || !openList.Contains(neighbours[i]))
        {
            neighbours[i].GCost = newMovementCost;
            neighbours[i].HCost = CalculateDistance(neighbours[i].gridPos, endNode.gridPos);

            neighbours[i].parent = currentNode;
            if (!openList.Contains(neighbours[i]))
            {
                openList.Add(neighbours[i]);
                newImprovedClosedList.Add(neighbours[i]);
            }
        }
    }

    private bool DidReachTarget()
    {
        Debug.Log("YOUVE FOUND THE PATH");
        return currentNode == endNode;
    }
    /// <summary>
    /// Resets all of the lists and variables the next time we try to find a path.
    /// </summary>
    /// <param name="startNodeGrid"></param>
    /// <param name="endNodeGrid"></param>
    /// <returns></returns>
    private bool Initialize(Vector3Int startNodeGrid, Vector3Int endNodeGrid)
    {
        openList.Clear();

        for (int i = 0; i < newImprovedClosedList.Count; i++)
        {
            newImprovedClosedList[i].parent = null;//Clear each node's parent so the next time we run the aglorithom does not get screwed by that data, similar to clearing the lists.
            newImprovedClosedList[i].wasVisited = false;//Again, simply resetting all the data for all the nodes.
        }
        //newImprovedClosedList.Clear();
        neighbours.Clear();
        finalPath.Clear();

        currentNode = null;

        startNode = grid.GetNode(startNodeGrid);
        endNode = grid.GetNode(endNodeGrid);
        currentNode = startNode;
        openList.Add(currentNode);
        //Debug.Log("Everything cleared");
        return !(startNode is null || endNode is null);
    }

    /// <summary>
    /// To get the new set of neighbours everytime we move on to the next 'current node'.
    /// </summary>
    /// <param name="currentNode"></param>
    private void GetNeighbours(Node currentNode)//
    {
        neighbours.Clear();//So every time you call GetNeighbours(); in the FindPath(); loop the list resets for the next 'current node'.
        for (int z = -1; z <= 1; z++)//Getting all the neighbours.
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3Int neighbourNodeVec = currentNode.gridPos + new Vector3Int(x, 0, z);

                if (neighbourNodeVec.x >= 0 && neighbourNodeVec.x < grid.cellCountX && neighbourNodeVec.z >= 0 && neighbourNodeVec.z < grid.cellCountZ)
                {
                    neighbours.Add(grid.GetNode(neighbourNodeVec));
                }
            }
        }
    }
    //
    private int CalculateDistance(Vector3 a, Vector3 b)//For checking if the new alternate path for the neighbour to start node is cheaper.
    {
        return (int)Mathf.Abs(b.x - a.x) + (int)Mathf.Abs(b.z - a.z);//.Abs(); gives absolute value so it is always positive.
    }

    private void GetPath(Node node)
    {
        finalPath.Add(node);

        Node parent = node.parent;

        if (parent != null)//Recurring function, it loops itself, the node you put in as the parameter gets the parent for itself and adds it to the list and so on.
        {
            GetPath(parent);
        }
    }
  
}
