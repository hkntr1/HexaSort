using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaveController : MonoBehaviour
{   
    Camera mainCamera;
    public GameObject target;
    private Vector3 hitPointLeft, hitPointRight;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private int numberOfTargets = 3; // Hedef nesne sayısı
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
        // Kameranın ekranın en solundan ve en sağından rayleri atıyoruz
        Ray leftRay = mainCamera.ScreenPointToRay(new Vector3(0, Screen.height / 5, 0));
        Ray rightRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width, Screen.height / 5, 0));

        RaycastHit hitLeft, hitRight;

        // Raylerin çarptığı noktaları buluyoruz
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
        float targetSpacing = spawnWidth / (numberOfTargets - 1); // Hedefler arası mesafe

        // Sağdan ve soldan boşluk
        float sideMarginWidth = spawnWidth * sideMargin;
        float usableWidth = spawnWidth - (2 * sideMarginWidth);

        for (int i = 0; i < numberOfTargets; i++)
        {
            // Hedeflerin x konumları hesaplanıyor
            float xPos = hitPointLeft.x + sideMarginWidth + (usableWidth / (numberOfTargets - 1)) * i;

            // Spawn noktası
            Vector3 spawnPoint = new Vector3(xPos, hitPointRight.y, hitPointRight.z);
            spawnPoints.Add(spawnPoint);
            // Hedef nesnesi spawn ediliyor
            Instantiate(target, spawnPoint, Quaternion.identity);
        }
    }
}

