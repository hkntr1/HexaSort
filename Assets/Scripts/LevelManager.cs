using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GridManager> gridManagers;
    public static Action onCheckNeeded;
    private void Start() {
        FindNeighbours();
        onCheckNeeded += CheckAll;   
    }
    public void FindNeighbours()
    {
        foreach (var gridManager in gridManagers)
        {
            gridManager.CheckNeighbours();
        }
    }
    public void CheckAll()
    {
        foreach (var gridManager in gridManagers)
        {
            gridManager.CheckColorMatch();
        }
    }  
     
}
