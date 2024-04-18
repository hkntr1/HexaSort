using UnityEngine;
using System;
public class ObjectSnap : MonoBehaviour
{
    private GameObject selectedObject;
    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 selectedObjectOriginalPosition;
    public Renderer selectedGrid; 
    public LayerMask stackObjectLayerMask,gridLayerMask;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit,5f, stackObjectLayerMask))
            {
                selectedObject = hit.transform.parent.gameObject;
                selectedObjectOriginalPosition = selectedObject.transform.position;
                offset = selectedObject.transform.position - hit.point;
                isDragging = true;
                Vector3 newPosition = hit.transform.position;
                newPosition.y = selectedObject.transform.position.y+0.01f; // Y eksenini sabit tut
                selectedObject.transform.position = newPosition;
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {   
            RaycastHit groundHit;
            Ray groundRay = new Ray(selectedObject.transform.position, Vector3.down);                   
            if (Physics.Raycast(groundRay, out groundHit, Mathf.Infinity))
            {
                if(groundHit.collider.CompareTag("Tile"))
                {    
                    SelectGrid(groundHit);
                }
                else
                {
                    CancelGrid();
                }
            }
          
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit,5f))
            {    
                Vector3 newPosition = hit.point + offset;
                newPosition.y = selectedObject.transform.position.y; // Y eksenini sabit tut
                selectedObject.transform.position = newPosition;
            }
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            CancelGrid();
            isDragging = false;
            RaycastHit groundHit;
            Ray groundRay = new Ray(selectedObject.transform.position, Vector3.down);                   
            if (Physics.Raycast(groundRay, out groundHit, Mathf.Infinity))
            { 

                if(groundHit.collider.CompareTag("Tile"))
                {  
                        WaveController.instance.stacks.Remove(selectedObject.GetComponent<Stack>());
                        WaveController.onItemCollected?.Invoke();
                        selectedObject.transform.position = groundHit.transform.position+Vector3.up*0.02f;
                        selectedObject = null;
                }    
                else
                {     
                    selectedObject.transform.position = selectedObjectOriginalPosition;
                    selectedObject = null;
                }
            }
            else
            {     
                selectedObject.transform.position = selectedObjectOriginalPosition; 
                selectedObject = null;
            }
            
        }
    }
    void SelectGrid(RaycastHit hit)
    {
        if(hit.transform.gameObject.GetComponent<Renderer>() == selectedGrid) return;
        CancelGrid();
        selectedGrid = hit.transform.gameObject.GetComponent<Renderer>();
        selectedGrid.material.color = Color.red;
    }
    void CancelGrid()
    {
        if(selectedGrid == null) return;
        selectedGrid.material.color = new Color(0.5766286f,0.7044498f,0.8773585f,1f);
        selectedGrid = null;
    }
}
