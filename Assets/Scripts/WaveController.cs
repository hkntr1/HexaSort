using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaveController : MonoBehaviour
{   
    Camera mainCamera;
    public GameObject target;
    private Vector3 hitPointLeft, hitPointRight;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int numberOfTargets = 3; 
    [SerializeField] private float sideMargin = 0.2f;
    public List<Vector3> spawnPoints;

    void Start()
    {
        mainCamera = Camera.main;
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
            Debug.Log(hitPointLeft);
        }

        if (Physics.Raycast(rightRay, out hitRight, Mathf.Infinity, targetLayer))
        {
            hitPointRight = hitRight.point;
            Debug.Log(hitPointRight);
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
            Vector3 spawnPoint = new Vector3(xPos, hitPointRight.y+0.02f, hitPointRight.z);
            spawnPoints.Add(spawnPoint);
            Instantiate(target, spawnPoint, Quaternion.identity);
        }
    }
}

