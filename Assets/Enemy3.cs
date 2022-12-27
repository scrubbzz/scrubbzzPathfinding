using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
/// <summary>
/// every update Move along path if path is avialable
/// if( last node is not equal to player){
/// find a new path
///     checking if there apth currently getting calulauted
///     if not
///         Get the path in the trhread
///         start a new thread to find path 
/// }
/// </summary>
public class Enemy3 : MonoBehaviour
{
    #region Old Version
    public AStar aStar;
    #endregion

    #region New Version
    AStar PathFinder;
    protected int cellCountXX = 50;
    int cellCountZ = 50;

    int cellSizeX = 1;
    int cellSizeZ = 1;

    public int chaseRange;
    #endregion
    //public Vector3Int start;
    //public Vector3Int end;

    public Rigidbody rb;//rb of agent.
    public Transform targetTransform;//Object the agent is trying to move to.
    public int targetNodeIndex;
    public float moveSpeed;

    public Vector3 previousTargetPosition;//Agent needs to move to a specific spot the target was on at somepoint, this variable saves that point.

    Thread myThread;
    ThreadStart threadStart;

    public bool threadStarted = false;
    public bool boolButton;

    Vector3 position;
    private void Awake()
    {
        //myThread = new Thread(new ParameterizedThreadStart( PathFinder.FindPath(Vector3Int.RoundToInt(this.transform.position), Vector3Int.RoundToInt(previousTargetTransform))));
        //myThread = new Thread(delegate { PathFinder.FindPath(Vector3Int.RoundToInt(this.transform.position), Vector3Int.RoundToInt(previousTargetTransform)); });
    }
    // Start is called before the first frame update
    void Start()
    {
        chaseRange = 10;
        rb = GetComponent<Rigidbody>();
        previousTargetPosition = targetTransform.position;

        PathFinder = new AStar();
        PathFinder.grid = new Grid(50, 50);
        if (myThread == null)
        {
            Debug.Log("MY THREAD IS NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {/*
        if (boolButton)
        {
            myThread.Start();
        }*/
        position = transform.position;
        if (CheckIfPlayerMoved())
        {
            //PathFinder.FindPath(Vector3Int.RoundToInt(this.transform.position), Vector3Int.RoundToInt(previousTargetTransform));
            if (threadStarted == false)
            {
                threadStarted = true;
                myThread?.Abort();
                myThread = new Thread(() =>
                {
                    PathFinder.FindPath(Vector3Int.RoundToInt(position), Vector3Int.RoundToInt(previousTargetPosition));
                    threadStarted = false;
                });
                myThread.Start();
                Debug.Log("Is the thread alive?: " + myThread.IsAlive);
                //myThread.Join();
            }
            else
            {
                Debug.Log("The thread is alive!!!!");
            }
        }

        if (PathFinder.finalPath.Count > 0)
        {
            Debug.Log("YOUVE FOUND A PATH");
            MoveAlongPath(rb);

            if (transform.position == PathFinder.finalPath[PathFinder.finalPath.Count - 1].worldPos)
            {
                rb.velocity = Vector3.zero;
                //Debug.Log("Enemy distance is " + Vector3.Distance(this.transform.position, targetTransform.position)); 
            }
        }
    }
    /// <summary>
    /// Has the target moved far away enough to warrant a new path being found?
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public bool CheckIfPlayerMoved()
    {
        //Debug.Log("Searching for player");
        if (Vector3.Distance(previousTargetPosition, targetTransform.position) >= chaseRange)
        {
            //Debug.Log("Target moved far away");
            previousTargetPosition = targetTransform.position;
            targetNodeIndex = 0;
            return true;

        }
        else
        {
            return false;
        }
    }
    public void MoveAlongPath(Rigidbody pathFollower)
    {
        pathFollower.transform.position = Vector3.MoveTowards(pathFollower.transform.position, PathFinder.finalPath[targetNodeIndex].worldPos, moveSpeed * Time.deltaTime);
        if ((transform.position - PathFinder.finalPath[targetNodeIndex].worldPos).magnitude <= 0.01f)//Is agent pos roughly the same as index pos?
        {
            if (targetNodeIndex < PathFinder.finalPath.Count - 1)
            {
                targetNodeIndex++;
                //Debug.Log("Target node index is " + targetNodeIndex);
            }
        }
    }

    public void ThreadFunction()
    {
        Debug.Log("This thread works");
    }

    public Thread StartTheThread(Vector3Int start, Vector3Int end)
    {
        var t = new Thread(() => PathFinder.FindPath(start, end));
        t.Start();
        Debug.Log("Eureaka");
        return t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 1, 1f);
        for (int x = 0; x < cellCountXX + 1; x++)
        {
            Gizmos.DrawLine((Vector3.zero + new Vector3(x * cellSizeX, 0, 0)), new Vector3(x * cellSizeX, 0, cellSizeZ * cellCountZ));
        }
        for (int z = 0; z < cellCountZ + 1; z++)
        {
            Gizmos.DrawLine((Vector3.zero + new Vector3(0, 0, z * cellSizeZ)), new Vector3(cellSizeX * cellCountXX, 0, z * cellSizeZ));
        }
        /* if (PathFinder.finalPath.Count > 0)
         {
             for (int i = 0; i < PathFinder.finalPath.Count; i++)
             {
                 Gizmos.color = Color.white;
                 Gizmos.DrawSphere(PathFinder.finalPath[i].worldPos, (PathFinder.grid.cellSizeX * PathFinder.grid.cellSizeZ) / 3f);

             }

         }*/
    }
}
