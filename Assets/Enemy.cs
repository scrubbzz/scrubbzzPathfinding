using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public AStar aStar;
    //private int index;
    public Vector3Int start;
    public Vector3Int end;

    //public bool shouldMove;
    public Rigidbody rb;
    public float moveSpeed;
    public int targetNodeIndex;//Refers to the index in the finalPath list that we want the agent to move along...
    public Transform targetTransform;

    public bool shouldMove;
    public Vector3 previousTargetTransform;
    // Start is called before the first frame update
    public bool findPath;
    void Start()
    {
        //index = 0;
        rb = transform.GetComponent<Rigidbody>();
        previousTargetTransform = targetTransform.position;
        findPath = true;
    }


    // Update is called once per frame
    void Update()
    {

        /* if (Input.GetKeyDown(KeyCode.Space))
         {
             aStar.FindPath(start, Vector3Int.RoundToInt(targetTransform.position)*//* end*//*);
             Debug.Log("your count is " + aStar.finalPath.Count);
         }*/
        /* if (rb.velocity.magnitude > 0)
         {
             aStar.FindPath(*//*Vector3Int.RoundToInt(this.transform.position),*//* start , Vector3Int.RoundToInt(targetTransform.position)*//* end*//*);
         }*/

        /* if (aStar.finalPath.Count == 0)
         {
             return;
         }*/
        //transform.position = Vector3.Lerp(transform.position, aStar.finalPath[targetNodeIndex].worldPos, moveSpeed * Time.deltaTime);//Lerp has trouble with determining the equality of two floats I guess...
        /* if ((transform.position - targetTransform.position).magnitude <= 5)
         {
             shouldMove = false;
         }
         else
         {
             shouldMove = true;
         }*/
        if (Vector3.Distance(previousTargetTransform, targetTransform.position) >= 10 && aStar.finalPath.Count > 0 && (transform.position - aStar.finalPath[aStar.finalPath.Count - 1].worldPos).magnitude < 0.1f)
        {
            findPath = true;
           /* shouldMove = true;*/
        }
        CheckIfPlayerMoved();
        if (aStar.finalPath.Count > 0)
        {
            MoveOnPath(rb);
        }
        /* if (aStar.finalPath.Count > 0)//If you do this then the bool will always be true, 'MoveAlongPath' keeps getting called, for loop inside
         {                               //it keeps refreshing, the agent gets stuck looping fully through the list over and over, each frame.
             shouldMove = true;

         }
         if (shouldMove)
         {
             MoveAlongPath(aStar.finalPath);
         }*/
    }
    private void CheckIfPlayerMoved()
    {
        if (Vector3.Distance(previousTargetTransform, targetTransform.position) >= 10 && findPath)
        {
            //previousTargetTransform = targetTransform.position;
            Debug.Log("previous transform is at " + previousTargetTransform);
            
            aStar.FindPath(Vector3Int.RoundToInt(this.transform.position),/* start,*/ Vector3Int.RoundToInt(previousTargetTransform)/* end*/);
            targetNodeIndex = 0;
            findPath = false;
            print("I hate myseklf");
        }
    }
    private void MoveOnPath(Rigidbody pathFollower)
    {
        /* if (shouldMove)
         {*/
        if (Vector3.Distance(transform.position, aStar.finalPath[targetNodeIndex].worldPos) < 0.2f)
        {

            //transform.position = Vector3.MoveTowards(transform.position, aStar.finalPath[targetNodeIndex].worldPos, moveSpeed * Time.deltaTime);
            pathFollower.transform.position = Vector3.MoveTowards(pathFollower.transform.position, aStar.finalPath[targetNodeIndex + 1].worldPos, moveSpeed * Time.deltaTime);

            if ((transform.position - aStar.finalPath[targetNodeIndex + 1].worldPos).magnitude <= 0.01f)//Is agent pos roughly the same as index pos?
            {
                if (targetNodeIndex < aStar.finalPath.Count - 1)
                {
                    targetNodeIndex++;
                    Debug.Log("Target node index is " + targetNodeIndex);
                }
            }

            if ((transform.position - aStar.finalPath[aStar.finalPath.Count - 1].worldPos).magnitude <= 1f)
            {
                rb.velocity = Vector3.zero;
            }
            else
            {
                rb.velocity = rb.velocity;
            }

            //shouldMove = false;

            //}

        }
       
    }
}