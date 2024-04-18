using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Stack : MonoBehaviour
{
    public int MaxStackCount,MinStackCount;
    public List<StackTile> stackList;   
    public bool isPlaced;
    [SerializeField] private StackObject stack;
    public List<StackTile> stackTiles;
    public List<StackObject> stackTilesObjects;
    void Start()
    {
       CreateStack();
    }
    void CreateStack()
    {
        int stackCount = Random.Range(MinStackCount, MaxStackCount);
        int firstSection=Random.Range(0, stackCount);
        int secondSection=Random.Range(firstSection, stackCount);
        int colorIndex = Random.Range(0, stackList.Count);
        int oldColorIndex = colorIndex;
        for (int i = 0; i < stackCount; i++)
        {
            StackObject stackObject = Instantiate(stack);
            stackObject.Init(stackList[colorIndex]);
            stackTilesObjects.Add(stackObject);
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
  public void AddStackTileToStack(StackObject newTile)
    {
        newTile.transform.parent = transform;
        newTile.transform.DOLocalMove(new Vector3(0, stackTilesObjects.Count * 0.03f, 0),0.3f);
        //newTile.transform.localPosition = ;
        stackTilesObjects.Add(newTile);
    }
   
   public void TransferToOtherStack(Stack otherStack)
   {
     Color color = stackTilesObjects[stackTilesObjects.Count-1].color;
     for (int i = stackTilesObjects.Count-1; i >= 0; i--)
     { 
      if(stackTilesObjects[i].color==color)
      {
        otherStack.AddStackTileToStack(stackTilesObjects[i]);
        stackTilesObjects.RemoveAt(i);
        
      } 
      else return;
     }
   }
   public void CheckBoom()
   {
    if(stackTilesObjects.Count==0)
    {
      return;
    }
    Color color = stackTilesObjects[stackTilesObjects.Count-1].color;
    List<StackObject> stackObjects = new List<StackObject>();
    for (int i = stackTilesObjects.Count-1; i >= 0; i--)
     { 
      if(stackTilesObjects[i].color==color)
      {
        stackObjects.Add(stackTilesObjects[i]);
      }
     }
    if (stackObjects.Count >= 10)
    {
      foreach (var stackObject in stackObjects)
      {
        ScoreManager.instance.ChangeScore(1);
        stackTilesObjects.Remove(stackObject);
        stackObject.transform.DOScaleX(0, 0.3f)
          .OnStart(() => stackObject.transform.DOScaleY(0, 0.3f))
          .OnComplete(() => Destroy(stackObject.gameObject));
      }
      LevelManager.onCheckNeeded.Invoke();
    }
   }
}
