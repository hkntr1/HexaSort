using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GridManager> gridManagers;
    public static Action onCheckNeeded;
    #region Singleton
	public static LevelManager instance;
	void Awake()
	{
		instance = this;
	}
	#endregion
    
    private void Start() {
      
        Init();
        onCheckNeeded += CheckAll;   
    }
    
    public void Init()
    { 
        gridManagers.Clear();
        foreach(GridManager gridManager in GameManager.instance.levels[GameManager.instance.currentLevel].gridManagers)
        {
            gridManagers.Add(gridManager);
        }
       
      //  gridManagers=GameManager.instance.levels[GameManager.instance.currentLevel].gridManagers;
        FindNeighbours();
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
    public void CheckBoomAll()
    {
        foreach (var gridManager in gridManagers)
        {
            gridManager.CurrentStack?.CheckBoom();
        }
    }
     
}
