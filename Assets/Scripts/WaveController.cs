using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaveController : MonoBehaviour
{   
    Camera mainCamera;
    public Stack stack;
    private Vector3 hitPointLeft, hitPointRight;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int numberOfTargets = 3; 
    [SerializeField] private float sideMargin = 0.2f;
    public List<Stack> stacks;
    public List<Vector3> spawnPoints;
    
    #region Singleton
    public static WaveController instance;
    void Awake()
    {
        instance = this;
    }
    #endregion
    
    public static Action<Stack> onItemCollected;
    
    void Start()
    { 
        onItemCollected += (Stack selectedStack) =>{
            stacks.Remove(selectedStack);
            if(stacks.Count == 0)
            {
                CreateNewWave();
            }
        };
        mainCamera = Camera.main;
        CalculateHitPoint();
        CreateSpawnPoints();
    }
    public void ResetWave()
    {
        foreach (var stack in stacks)
        {
            Destroy(stack.gameObject);
        }
        stacks.Clear();
        spawnPoints.Clear();
        CalculateHitPoint();
        CreateSpawnPoints();
    }
    void CalculateHitPoint()
    {
        Ray leftRay = mainCamera.ScreenPointToRay(new Vector3(0, Screen.height / 5, 0));
        Ray rightRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width, Screen.height / 5, 0));

        RaycastHit hitLeft, hitRight;

        if (Physics.Raycast(leftRay, out hitLeft, Mathf.Infinity, targetLayer))
        {
            hitPointLeft = hitLeft.point;
        }
        if (Physics.Raycast(rightRay, out hitRight, Mathf.Infinity, targetLayer))
        {
            hitPointRight = hitRight.point;
        }
    }

    void CreateSpawnPoints()
    {
        float spawnWidth = Mathf.Abs(hitPointRight.x - hitPointLeft.x);
        float targetSpacing = spawnWidth / (numberOfTargets - 1); 

        float sideMarginWidth = spawnWidth * sideMargin;
        float usableWidth = spawnWidth - (2 * sideMarginWidth);

        for (int i = 0; i < numberOfTargets; i++)
        {    
            float xPos = hitPointLeft.x + sideMarginWidth + (usableWidth / (numberOfTargets - 1)) * i;
            Vector3 spawnPoint = new Vector3(xPos, hitPointRight.y + 0.02f, hitPointRight.z);
            spawnPoints.Add(spawnPoint);
            Stack newStack = Instantiate(stack, spawnPoint, Quaternion.identity);
            stacks.Add(newStack);
        }
    }
    
    void CreateNewWave()
    {
        StartCoroutine(SpawnCoroutine());    
    }
    
    IEnumerator SpawnCoroutine()
    {
        for (int i = 0; i < numberOfTargets; i++)
        {   
            Stack newStack = Instantiate(stack, spawnPoints[i] + Vector3.right, Quaternion.identity);
            newStack.transform.DOMove(spawnPoints[i], 0.5f).onComplete += () =>  
            newStack.transform.DOScale(Vector3.one * 1.1f, 0.1f).OnComplete(() =>
            {
               newStack.transform.DOScale(Vector3.one, 0.1f);
            });
            stacks.Add(newStack);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
