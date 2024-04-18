using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
   public bool isEmpty;
   public Stack CurrentStack;
    public List<GridManager> neighborHexagons = new List<GridManager>();

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
    public void CheckColorMatch()
   {
        Debug.Log("Checking Color Match");
         if (neighborHexagons.Count == 0)
         {
              return;
         }
         foreach (GridManager neighbor in neighborHexagons)
         {
              if (neighbor.CurrentStack != null&&CurrentStack!=null)
              {
                if (neighbor.CurrentStack.stackTilesObjects.Count == 0 )
                {
                    neighbor.CurrentStack=null;
                    return;
                }
                if (CurrentStack.stackTilesObjects.Count == 0)
                {
                    CurrentStack=null;
                    return;
                }
                if (neighbor.CurrentStack.stackTilesObjects[neighbor.CurrentStack.stackTilesObjects.Count-1].color == CurrentStack.stackTilesObjects[CurrentStack.stackTilesObjects.Count-1].color)
                {
                   neighbor.CurrentStack.TransferToOtherStack(CurrentStack);
                }
              }
         }
   }    
}
