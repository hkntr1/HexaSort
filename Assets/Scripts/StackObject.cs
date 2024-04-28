using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackObject : MonoBehaviour
{
    public Color color;
    public string name;
    ParticleSystem particle;
    public void Init(StackTile stackTile)
    {
        color = stackTile.color;
        name = stackTile.name;  
    
        GetComponent<MeshRenderer>().material.color = color;
        gameObject.name = name; 
        gameObject.SetActive(true);
    }
}
