using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid
{
    /// <summary>
    /// This class simply creates an array of nodes that act as the grid to be used for the finding the path.
    /// </summary>

    public int cellCountX => nodesArray.GetLength(0);
    public int cellCountZ => nodesArray.GetLength(1);

    public int cellSizeX = 1;
    public int cellSizeZ = 1;

    public Node[,] nodesArray;

    /// <summary>
    /// Parameter inputs used to create the grid.
    /// </summary>
    /// <param name="cellCountX"></param>
    /// <param name="cellCountZ"></param>
    public Grid(int cellCountX, int cellCountZ)
    {
        CreateNodeGrid(cellCountX, cellCountZ);

    }

    // Start is called before the first frame update
    void Start()
    {
        //CreateNodeGrid(cellCountX, cellCountZ);
    }

    public int CreateNodeGrid(int cellCountX, int cellCountZ)
    {
        nodesArray = new Node[cellCountX , cellCountZ];

        for (int z = 0; z < cellCountZ; z++)//looping through each node in the array and setting its position...
        {
            for (int x = 0; x < cellCountX; x++)
            {
                nodesArray[x,z] = new Node(new Vector3(x * cellSizeX + (cellSizeX / 2.0f), 0, z * cellSizeZ + (cellSizeZ / 2.0f)), new Vector3Int(x, 0, z), true);

                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.position = nodesArray[x,z].worldPos;
                go.transform.localScale = new Vector3(cellSizeX, 0, cellSizeZ) / 3f; /// 2.0f;
                go.GetComponent<Collider>().enabled = false;
            }
        }
        return nodesArray.Length;
    }

    public Node GetNode(Vector3Int gridPosition)//Obtain a specific node on the grid...
    {
        //Debug.Log("CellcountX is " + cellCountX);
        //Debug.Log("You found node no. " + gridPosition.x + "," + gridPosition.z);
        //Console.WriteLine("You found node no. " + gridPosition.x, gridPosition.z);
        if (gridPosition.x >= nodesArray.GetLength(0) || gridPosition.y >= nodesArray.GetLength(1))
        {
            return null;
        }

        return nodesArray[gridPosition.x, gridPosition.z];
    }
    public void PrintMyMessage()
    {
        Debug.Log("You Suck Hasan");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 25f);
        for (int x = 0; x < cellCountX + 1; x++)
        {
            Gizmos.DrawLine((Vector3.zero + new Vector3(x * cellSizeX, 0, 0)), new Vector3(x * cellSizeX, 0, cellSizeZ * cellCountZ));
        }
        for (int z = 0; z < cellCountZ + 1; z++)
        {
            Gizmos.DrawLine((Vector3.zero + new Vector3(0, 0, z * cellSizeZ)), new Vector3(cellSizeX * cellCountX, 0, z * cellSizeZ));
        }
    }

}
