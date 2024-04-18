using UnityEngine;

[CreateAssetMenu(fileName = "StackTile", menuName = "StackTile", order = 0)]
public class StackTile : ScriptableObject {
    
    public Color color;
    public string name;
    
    public int colorIndex;
}
