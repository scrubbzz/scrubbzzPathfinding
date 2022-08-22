using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    public AStar aStar;
    public Vector3Int start;
    public Vector3Int end;

    public Rigidbody rb;//rb of agent.
    public Transform targetTransform;//Object the agent is trying to move to.
    public int targetNodeIndex;
    public float moveSpeed;

    public Vector3 previousTargetTransform;//Agent needs to move to a specific spot the target was on at somepoint, this variable saves that point.
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousTargetTransform = targetTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerMovedAndFindPath(Vector3Int.RoundToInt(this.transform.position), Vector3Int.RoundToInt(previousTargetTransform));
        if (aStar.finalPath.Count > 0)
        {
            MoveAlongPath(rb);
            if(transform.position == aStar.finalPath[aStar.finalPath.Count - 1].worldPos)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
    public void CheckIfPlayerMovedAndFindPath(Vector3Int start, Vector3Int end)
    {
        if (Vector3.Distance(previousTargetTransform, targetTransform.position) >= 5)//Has the target moved far away enough to warrant a new path being found?
        {                                                                                                     //The number 10 could really be any value we want.
            previousTargetTransform = targetTransform.position;
            targetNodeIndex = 0;
            aStar.FindPath(/*Vector3Int.RoundToInt(this.transform.position)*/ start, end /*Vector3Int.RoundToInt(previousTargetTransform*/);
        }
    }
    public void MoveAlongPath(Rigidbody pathFollower)
    {
        pathFollower.transform.position = Vector3.MoveTowards(pathFollower.transform.position, aStar.finalPath[targetNodeIndex].worldPos, moveSpeed * Time.deltaTime);
        if ((transform.position - aStar.finalPath[targetNodeIndex].worldPos).magnitude <= 0.01f)//Is agent pos roughly the same as index pos?
        {
            if (targetNodeIndex < aStar.finalPath.Count - 1)
            {
                targetNodeIndex++;
                Debug.Log("Target node index is " + targetNodeIndex);
            }
        }
    }
}
