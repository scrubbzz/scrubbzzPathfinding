using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public AStar aStar;
    //private int index;
    public Vector3Int start;
    public Vector3Int end;

   
   
    // Start is called before the first frame update
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindNewPath();
    }
    public void FindNewPath()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            aStar.FindPath(start, end);
            print("I hate myseklf");
        }
    }
}
