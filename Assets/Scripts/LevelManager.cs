using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GridManager> gridManagers;
    UiController uiController;
    public static Action onCheckNeeded;
    #region Singleton
	public static LevelManager instance;
    public GameManager gameManager; 
	void Awake()
	{
		instance = this;
        
	}
	#endregion
    
    private void Start() {
      
        uiController = FindObjectOfType<UiController>();
        Init();
        onCheckNeeded += CheckAll;   
    }
    
    public void Init()
    {   
       
        gridManagers.Clear();
        foreach(GridManager gridManager in gameManager.currentLevelData.gridManagers)
        {
            gridManagers.Add(gridManager);
        }
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
    public void ClearStacks()
    {
        foreach (var gridManager in gridManagers)
        {
            if(gridManager.CurrentStack!=null)
            {
                Destroy(gridManager.CurrentStack.gameObject);
            }
        }
    }
    public void CheckBoomAll()
    {
        foreach (var gridManager in gridManagers)
        {
            gridManager.CurrentStack?.CheckBoom();
        }
    }
    public void CheckFail()
    {
       
        foreach (var gridManager in gridManagers)
        {
            if(gridManager.CurrentStack!=null)
            {
                Debug.Log("Still Empty");
                return;
            }  
            Debug.Log("Fail");
            uiController.FailScreen();
        }
      
    } 
}
