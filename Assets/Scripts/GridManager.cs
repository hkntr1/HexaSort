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
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, direction, out hit, 1f))
            {
                
                Transform hitTransform = hit.transform;

                if (hitTransform.CompareTag("Tile"))
                {
                    neighborHexagons.Add(hitTransform.GetComponent<GridManager>());
                }
            }
   }
   
}
    public void CheckColorMatch()
   {
        
         if (neighborHexagons.Count == 0)
         {
              return;
         }
         foreach (GridManager neighbor in neighborHexagons)
         {
              if (neighbor.CurrentStack != null&&CurrentStack!=null)
              {
                if (CurrentStack.stackTilesObjects.Count!=0&&neighbor.CurrentStack.stackTilesObjects.Count!=0)
                {
                if (neighbor.CurrentStack.stackTilesObjects[neighbor.CurrentStack.stackTilesObjects.Count-1].color == CurrentStack.stackTilesObjects[CurrentStack.stackTilesObjects.Count-1].color)
                { 
                   neighbor.CurrentStack.TransferToOtherStack(CurrentStack);
                }
                }
              }
         }
       
   }    
}
