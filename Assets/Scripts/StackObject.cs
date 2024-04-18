using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackObject : MonoBehaviour
{
    public Color color;
    public string name;
    public int colorIndex;
    public void Init(StackTile stackTile)
    {
        color = stackTile.color;
        name = stackTile.name;  
        colorIndex = stackTile.colorIndex;
        GetComponent<MeshRenderer>().material.color = color;
        gameObject.name = name; 
    }
}
