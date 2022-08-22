using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [SerializeField] LineDrawer grid;

    /*[SerializeField] Vector3Int startNodeGrid;
    [SerializeField] Vector3Int endNodeGrid;*/

    public Node startNode;
    public Node endNode;//The goal node.
    public Node currentNode;
    List<Node> neighbours = new List<Node>();//All of the neignbours in the immediate vicinity of the current node.

    List<Node> openList = new List<Node>();//Nodes still to be checked.
    List<Node> newImprovedClosedList = new List<Node>();//Nodes no longer needed to be checked.
    public List<Node> finalPath = new List<Node>();

    // Start is called before the first frame update
    void Start()
    {
        /* startNode = grid.GetNode(startNodeGrid);
         endNode = grid.GetNode(endNodeGrid);
 */
        /*  currentNode = startNode;
          openList.Add(currentNode);*/

        /* for (int z = -1; z <= 1; z++)//Getting all the neighbours.
         {
             for (int x = -1; x <= 1; x++)
             {
                 Vector3Int neighbourNodeVec = startNode.gridPos + new Vector3Int(x, 0, z);

                 if(neighbourNodeVec.x >= 0 && neighbourNodeVec.x < grid.cellCountX && neighbourNodeVec.z >= 0 && neighbourNodeVec.z < grid.cellCountZ)
                 {
                     neighbours.Add(grid.GetNode(neighbourNodeVec));
                 }
             }
         }*/

    }

    // Update is called once per frame
    void Update()
    {

        /* openList.Sort();
         currentNode = openList[0];
         openList.RemoveAt(0);
         //closedList.Add(currentNode);//Add function adds stuff to the end.
         currentNode = closedList[0];*/
        //RetracePath(startNode, endNode);
        //if (currentNode == endNode) return;//If current node is equal to the target (end) node then path has been found.

        /* for (int i = 0; i < neighbours.Count; i++)//UNDERSTAND THIS PART YOU FOOL.
         {
             if (neighbours[i].isTraversable && !closedList.Contains(neighbours[i]))//If you can move on the node and it has yet to be checked.
             {
                 if (!openList.Contains(neighbours[i]))
                 {
                     int newMovementCost = CalculateDistance(neighbours[i].gridPos, startNode.gridPos); //This is the distance of an alternate path the neighbour could take to the start node,
                                                                                                        //moving in a straight line rather than through its parent to the start node.
                    if(newMovementCost < neighbours[i].GCost || !openList.Contains(neighbours[i]))//is alternate path shorter than neighbour's current path?
                     {
                         neighbours[i].GCost = newMovementCost; 
                         neighbours[i].HCost = CalculateDistance(neighbours[i].gridPos, endNode.gridPos); 

                         neighbours[i].parent = currentNode;
                         if (!openList.Contains(neighbours[i]))
                         {
                             openList.Add(neighbours[i]);

                         }
                     } 

                 }
             }
         }*/
    }
    //
    public void GetNeighbours(Node currentNode)//To get the new set of neighbours everytime we move on to the next 'current node'.
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
    public int CalculateDistance(Vector3 a, Vector3 b)//For checking if the new alternate path for the neighbour to start node is cheaper.
    {
        return (int)Mathf.Abs(b.x - a.x) + (int)Mathf.Abs(b.z - a.z);//.Abs(); gives absolute value so it is always positive.
    }

    public void GetPath(Node node)
    {
        finalPath.Add(node);

        Node parent = node.parent;

        if (parent != null)//Recurring function, it loops itself, the node you put in as the parameter gets the parent for itself and adds it to the list and so on.
        {
            GetPath(parent);
        }
    }
    //
   /* public void RetracePath(Node startNode, Node endNode)//Reverses the final path list to get the nodes in the correct order for the agent to move along (NOT USING).
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        finalPath = path;
    }*/
    //
    public bool FindPath(Vector3Int startNodeGrid, Vector3Int endNodeGrid)
    {
        openList.Clear();

        for (int i = 0; i < newImprovedClosedList.Count; i++)
        {
            newImprovedClosedList[i].parent = null;//Clear each node's parent so the next time we run the aglorithom does not get screwed by that data, similar to clearing the lists.
            newImprovedClosedList[i].wasVisited = false;//Again, simply resetting all the data for all the nodes.
        }

        newImprovedClosedList.Clear();
        neighbours.Clear();
        finalPath.Clear();

        currentNode = null;

        startNode = grid.GetNode(startNodeGrid);
        endNode = grid.GetNode(endNodeGrid);
        currentNode = startNode;
        openList.Add(currentNode);

        while (openList.Count > 0)
        {
            //neighbours.Clear();
            openList.Sort();
            currentNode = openList[0];
            openList.RemoveAt(0);
            currentNode.wasVisited = true;//Each node has a bool check that is set as true so that we know we visited it for some data resetting later.
            newImprovedClosedList.Add(currentNode);//Add function adds stuff to the end.
            //currentNode = closedList[0];

            if (currentNode == endNode)
            {
                GetPath(endNode);//Gets the parent of each previously visited node and adds them to the 'finalPath' list.
                finalPath.Reverse();//To get the nodes in the correct order to actually travel on.
                return true;
            }
            GetNeighbours(currentNode);//Call this once rather than in the loop below so you don't get a million neighbours...
            for (int i = 0; i < neighbours.Count; i++)//UNDERSTAND THIS PART YOU FOOL.
            {
                if (neighbours[i].isTraversable && !neighbours[i].wasVisited)//If you can move on the node and it has yet to be checked.
                {
                    if (!openList.Contains(neighbours[i]))
                    {
                        int newMovementCost = CalculateDistance(neighbours[i].gridPos, startNode.gridPos); //This is the distance of an alternate path the neighbour could take to the start node,
                                                                                                           //moving in a straight line rather than through its parent to the start node.
                        if (newMovementCost < neighbours[i].GCost || !openList.Contains(neighbours[i]))//is alternate path shorter than neighbour's current path?
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
                }
            }


        }

        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;



       /* Gizmos.color = Color.green;
        Gizmos.DrawSphere(startNode.gridPos, 3f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endNode.worldPos, 3f);*/


        for (int i = 0; i < finalPath.Count; i++)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(finalPath[i].worldPos, (grid.cellSizeX * grid.cellSizeZ)/3f);

        }





    }
}
