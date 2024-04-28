using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GridManager> gridManagers;
    UiController uiController;
    bool isFail;
    
    public static Action onCheckNeeded;

    #region Singleton
    public static LevelManager instance;
    public GameManager gameManager;

    void Awake()
    {
        instance = this;
    }
    #endregion
    
    private void Start()
    {
        uiController = FindObjectOfType<UiController>();
        Init();
        onCheckNeeded += CheckAllColorMatch;
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
    
    public void CheckAllColorMatch()
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
            if(gridManager.CurrentStack != null)
            {
                Destroy(gridManager.CurrentStack.gameObject);
            }
        }
    }
    
    public void CheckBoomAll()
    {
        int count=0;                                     
        foreach (var gridManager in gridManagers)
        {
           
           if (gridManager.CurrentStack==null) 
            {
                count++;
              
            }
            else 
            {
                if(gridManager.CurrentStack.isMatching==false)
                {
                    count++;
                
                }
              
            }
          
        }
        if(count==gridManagers.Count)
        {
            Debug.Log("All Stacks are Cheching Boom");
            foreach (var gridManager in gridManagers)
            {
                gridManager.CurrentStack?.CheckBoom();
            }
       }
        
    }
    
    public void CheckFail()
    {
        if(isFail) return;  
        int freeCount = 0;
        for (int i = 0; i < gridManagers.Count; i++)
        {
            if(gridManagers[i].CurrentStack != null)
            {
                freeCount++;
            }
            if(freeCount == gridManagers.Count)
            {
                isFail = true;
                uiController.FailScreen();
            }
        }
    } 
}
