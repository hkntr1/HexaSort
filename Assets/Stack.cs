using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    public int MaxStackCount,MinStackCount;
    public List<StackTile> stackList;   
    [SerializeField] private GameObject stack;
    void Start()
    {
       // int stackVariety = Random.Range(0, 3);
        int stackCount = Random.Range(MinStackCount, MaxStackCount);
        Debug.Log(stackCount+":Stack Count");
        int firstSection=Random.Range(0, stackCount);
        Debug.Log(firstSection+":First Section");
        int secondSection=Random.Range(firstSection, stackCount);
        Debug.Log(secondSection+":second Section");
        int colorIndex = Random.Range(0, stackList.Count);
        Debug.Log(colorIndex+":Color Index");
        int oldColorIndex = colorIndex;
        for (int i = 0; i < stackCount; i++)
        {
            GameObject stackObject = Instantiate(stack);
            stackObject.GetComponent<MeshRenderer>().material.color = stackList[colorIndex].color;
            stackObject.name = stackList[colorIndex].name;
            if(i==firstSection)
            { 
              stackList.RemoveAt(colorIndex);
              colorIndex = Random.Range(0, stackList.Count);
            
            }
            else if(i==secondSection)
            {
              stackList.RemoveAt(colorIndex);
              colorIndex = Random.Range(0, stackList.Count);
            }  
            else if(i==stackCount-1)
            {
              stackList.RemoveAt(colorIndex);
            }
            float yPos=i*0.03f;
            stackObject.transform.parent = transform;
            stackObject.transform.localPosition = new Vector3(0, yPos, 0);
        }
    }

    
}
