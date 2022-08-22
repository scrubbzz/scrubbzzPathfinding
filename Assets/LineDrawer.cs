using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    /// <summary>
    /// This class simply creates an array of nodes that act as the grid to be used for the finding the path.
    /// </summary>

    public int cellCountX;
    public int cellCountZ;

    public int cellSizeX;
    public int cellSizeZ;

    Node[] nodes;

    // Start is called before the first frame update
    void Start()
    {
        CreateNodeGrid(cellCountX, cellCountZ);
    }

    private void CreateNodeGrid(int cellCountX, int cellCountZ)
    {
        nodes = new Node[cellCountX * cellCountZ];

        for (int z = 0; z < cellCountZ; z++)//looping through each node in the array and setting its position...
        {
            for (int x = 0; x < cellCountX; x++)
            {
                int i = x + z * cellCountX;
                nodes[i] = new Node(new Vector3(x * cellSizeX + (cellSizeX / 2.0f), 0, z * cellSizeZ + (cellSizeZ / 2.0f)), new Vector3Int(x, 0, z), true);

                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.position = nodes[i].worldPos;
                go.transform.localScale = new Vector3(cellSizeX, 0, cellSizeZ)/3f; /// 2.0f;
                go.GetComponent<Collider>().enabled = false;
            }
        }
    }

    public Node GetNode(Vector3Int gridPosition)//Obtain a specific node on the grid...
    {
        int i = gridPosition.x + gridPosition.z * cellCountX;
        return nodes[i];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 25f);
        for (int x = 0; x < cellCountX + 1; x++)
        {
            Gizmos.DrawLine((transform.position + new Vector3(x * cellSizeX, 0, 0)), new Vector3(x * cellSizeX, 0, cellSizeZ * cellCountZ));
        }
        for (int z = 0; z < cellCountZ + 1; z++)
        {
            Gizmos.DrawLine((transform.position + new Vector3(0, 0, z * cellSizeZ)), new Vector3(cellSizeX * cellCountX, 0, z * cellSizeZ));
        }
    }
   
}
