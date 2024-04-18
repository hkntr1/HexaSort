using UnityEngine;
using System;
public class ObjectSnap : MonoBehaviour
{
    private Stack selectedStack;
    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 selectedObjectOriginalPosition;
    public GameObject selectedGrid; 
    public LayerMask stackObjectLayerMask,gridLayerMask;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit,5f, stackObjectLayerMask))
            {
                
                selectedStack = hit.transform.parent.GetComponent<Stack>();
                if(selectedStack.isPlaced) return;
                selectedObjectOriginalPosition = selectedStack.transform.position;
                offset = selectedStack.transform.position - hit.point;
                isDragging = true;
                Vector3 newPosition = hit.transform.position;
                newPosition.y = selectedStack.transform.position.y+0.01f; // Y eksenini sabit tut
                selectedStack.transform.position = newPosition;
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {   
            RaycastHit groundHit;
            Ray groundRay = new Ray(selectedStack.transform.position, Vector3.down);                   
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
                newPosition.y = selectedStack.transform.position.y; // Y eksenini sabit tut
                selectedStack.transform.position = newPosition;
            }
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
           
            isDragging = false;
            if (selectedGrid!=null && selectedGrid.GetComponent<GridManager>().isEmpty)
            { 
                TruePlace();
            }    
            else
            {     
                WrongPlace();
            }
            CancelGrid();
        }
    }
    void SelectGrid(RaycastHit hit)
    {
        if(hit.transform.gameObject == selectedGrid) return;
        CancelGrid();
        selectedGrid = hit.transform.gameObject;
        selectedGrid.GetComponent<Renderer>().material.color = Color.red;
    }
    void CancelGrid()
    {
        if(selectedGrid == null) return;
        selectedGrid.GetComponent<Renderer>().material.color = new Color(0.5766286f,0.7044498f,0.8773585f,1f);
        selectedGrid = null;
    }
    void WrongPlace()
    {
        selectedStack.transform.position = selectedObjectOriginalPosition; 
        selectedStack = null;
    }
    void TruePlace()
    {
        selectedGrid.GetComponent<GridManager>().isEmpty = false;
        selectedGrid.GetComponent<GridManager>().CurrentStack = selectedStack.GetComponent<Stack>();
        selectedStack.isPlaced = true;
        WaveController.onItemCollected?.Invoke(selectedStack);
        selectedStack.transform.position = selectedGrid.transform.position+Vector3.up*0.02f;
        selectedStack = null;
        LevelManager.onCheckNeeded.Invoke();
    }
}
