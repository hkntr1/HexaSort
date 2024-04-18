using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
   public bool isEmpty;
   public Stack CurrentStack;
    List<GridManager> neighborHexagons = new List<GridManager>();
    public void CheckNeighbours()
   {
      float[] angles = { 30f, 90f, 150f, 210f, 270f, 330f };

        foreach (float angle in angles)
        {
            // Açıyı kullanarak ray oluştur
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            RaycastHit hit;

            // Ray'i ateşle, 100 birimlik bir mesafeye kadar
            if (Physics.Raycast(transform.position, direction, out hit, 1f))
            {
                // Çarpışan nesneyi al
                Transform hitTransform = hit.transform;

                // Eğer çarpışan nesne bir altıgense, komşu listesine ekle
                if (hitTransform.CompareTag("Tile"))
                {
                    neighborHexagons.Add(hitTransform.GetComponent<GridManager>());
                }
            }
   }
}
}
