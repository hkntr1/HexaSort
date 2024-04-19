using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Stack : MonoBehaviour
{
    public bool isMatching;
    public int MaxStackCount,MinStackCount;
    public List<StackTile> stackList;   
    public bool isPlaced;
    public GridManager currentGrid;
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
    if(otherStack.isMatching||isMatching)
    {
      return;
    }
    StartCoroutine(TransferCoroutine(otherStack));
   }
   IEnumerator TransferCoroutine(Stack otherStack )
   { 
    otherStack.isMatching=true;
    isMatching=true; 
    Color color = stackTilesObjects[stackTilesObjects.Count-1].color;
    for (int i = stackTilesObjects.Count-1; i >= 0; i--)
     { Debug.Log("Transfer: "+i);
      if(stackTilesObjects[i].color==color)
      {
        otherStack.AddStackTileToStack(stackTilesObjects[i]);
        stackTilesObjects.RemoveAt(i);
        yield return new WaitForSeconds(0.3f);
      } 
      else
      {
       otherStack.CheckBoom();
     
       break;
      } 
      isMatching=false;
      otherStack.isMatching=false;
    
     }
     LevelManager.onCheckNeeded.Invoke();
     CheckEmpty();
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
      StartCoroutine(PerformActionsCoroutine(stackObjects));
      LevelManager.onCheckNeeded.Invoke();
    }
   }
    IEnumerator PerformActionsCoroutine(List<StackObject> stackObjects)
    {
        foreach (var stackObject in stackObjects)
        {
            // Puanı arttır
            ScoreManager.instance.ChangeScore(1);

            // Obje listesinden kaldır
            stackTilesObjects.Remove(stackObject);

            // Obje küçültme animasyonu ve yok etme
            stackObject.transform.DOScale(Vector3.zero, 0.3f)
                .OnComplete(() => Destroy(stackObject.gameObject));

            // Bir sonraki obje için belirtilen gecikmeyi bekle
            yield return new WaitForSeconds(0.1f);
        }
        CheckEmpty();
        // Tüm objelerin işlemleri tamamlandığında LevelManager.onCheckNeeded.Invoke() çağır
        LevelManager.onCheckNeeded.Invoke();
    }
    public void CheckEmpty()
    {
      if(stackTilesObjects.Count==0)
      {
        currentGrid.isEmpty=true;
        currentGrid.CurrentStack=null;
        Destroy(gameObject);
      }
      else return;
    }
}

