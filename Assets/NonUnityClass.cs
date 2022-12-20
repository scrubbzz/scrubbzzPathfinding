using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonUnityClass 
{
    int x = 0;
  
    Grid line;
    public int NonMonobehvaiourFuction()
    {
        x++;
        Debug.Log("x value is now " + x);
        return x;
    }
}
