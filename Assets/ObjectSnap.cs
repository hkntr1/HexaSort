using UnityEngine;

public class ObjectSnap : MonoBehaviour
{
    private GameObject selectedObject;
    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 selectedObjectOriginalPosition;

    // Yalnızca belirli layer'daki objelerle çalışmak için
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
            isDragging = false;
            RaycastHit groundHit;
            Ray groundRay = new Ray(selectedObject.transform.position, Vector3.down);                   
            if (Physics.Raycast(groundRay, out groundHit, Mathf.Infinity))
            { 
                Debug.Log("0");
                if(groundHit.collider.CompareTag("Tile"))
                {  
                    Debug.Log("1");
                    RaycastHit hit;
                   
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity,gridLayerMask))
                    { 
                        Debug.Log(hit.collider.gameObject.name);    
                        selectedObject.transform.position = hit.transform.position+Vector3.up*0.02f;
                        selectedObject = null;
                    }   
                    else
                    {
                        Debug.Log("3");
                        selectedObject.transform.position = selectedObjectOriginalPosition;
                        selectedObject = null;
                    }
                }    
                else
                {
                    Debug.Log("4:"+ groundHit.collider.gameObject.name+" "+groundHit.collider.tag);
                    
                    selectedObject.transform.position = selectedObjectOriginalPosition;
                    selectedObject = null;
                }
            }
            else
            {
                Debug.Log("5");
                selectedObject.transform.position = selectedObjectOriginalPosition; 
                selectedObject = null;
            }
            
        }
    }
}
