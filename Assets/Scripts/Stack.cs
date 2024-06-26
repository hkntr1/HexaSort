using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stack : MonoBehaviour
{   
    public bool isMatching;
    public int MaxStackCount, MinStackCount;
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
        int firstSection = Random.Range(0, stackCount);
        int secondSection = Random.Range(firstSection, stackCount);
        int colorIndex = Random.Range(0, stackList.Count);
        int oldColorIndex = colorIndex;

        for (int i = 0; i < stackCount; i++)
        {
            StackObject stackObject = SimpleGameObjectPool.instance.GetObject(stack.gameObject).GetComponent<StackObject>();
            stackObject.Init(stackList[colorIndex]);
            stackTilesObjects.Add(stackObject);

            if (i == firstSection || i == secondSection || i == stackCount - 1)
            { 
                stackList.RemoveAt(colorIndex);
                colorIndex = Random.Range(0, stackList.Count);
            }

            float yPos = i * 0.015f;
            stackObject.transform.parent = transform;
            stackObject.transform.localPosition = new Vector3(0, yPos, 0);
        }
    }

    public void AddStackTileToStack(StackObject newTile)
    {
        Vector3 previousPosition = newTile.transform.position;
        Vector3 targetPosition = transform.position;
        Vector3 moveDirection = targetPosition - previousPosition;   
        newTile.transform.parent = transform;
        float localyPos = stackTilesObjects.Count * 0.015f;

        newTile.transform.DOLocalMove(new Vector3(0, localyPos + 0.15f, 0), 0.4f)
            .OnComplete(() => newTile.transform.DOLocalMove(new Vector3(0, localyPos, 0), 0.2f));

        if (moveDirection.x > 0.08f && moveDirection.x < 0.1f && moveDirection.z > 0.14f && moveDirection.z < 0.16f)
        {
            newTile.transform.DORotateQuaternion(new Quaternion(0.61f, -0.35f, -0.35f, -0.61f), 0.3f)
                .OnComplete(() => newTile.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0)));
        }
        else if (moveDirection.x < -0.08f && moveDirection.x > -0.1f && moveDirection.z < -0.14f && moveDirection.z > -0.16f)
        {
            newTile.transform.DORotateQuaternion(new Quaternion(-0.61f, 0.35f, 0.35f, 0.61f), 0.3f)
                .OnComplete(() => newTile.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0)));
        }
        else if (moveDirection.x > 0.16f && moveDirection.x < 0.18f && moveDirection.z > -0.01f && moveDirection.z < 0.1f)
        {
            newTile.transform.DORotateQuaternion(new Quaternion(0, -0.70f, -0.70f, 0), 0.3f)
                .OnComplete(() => newTile.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0)));
        } 
        else if (moveDirection.x < -0.16f && moveDirection.x > -0.18f && moveDirection.z > -0.01f && moveDirection.z < 0.1f)
        {
            newTile.transform.DORotateQuaternion(new Quaternion(0, 0.70f, 0.70f, 0), 0.3f)
                .OnComplete(() => newTile.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0)));
        } 
        else if (moveDirection.x > 0.08f && moveDirection.x < 0.1f && moveDirection.z < -0.14f && moveDirection.z > -0.16f)
        {
            newTile.transform.DORotateQuaternion(new Quaternion(-0.61f ,-0.35f,-0.35f,0.61f), 0.3f)
                .OnComplete(() => newTile.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0)));
        }
        else if (moveDirection.x < -0.08f && moveDirection.x > -0.1f && moveDirection.z > 0.14f && moveDirection.z < 0.16f)
        {
            newTile.transform.DORotateQuaternion(new Quaternion(0.61f, 0.35f, 0.35f, -0.61f), 0.3f)
                .OnComplete(() => newTile.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0)));
        }

        stackTilesObjects.Add(newTile);
    }
    public void TransferToOtherStack(Stack otherStack)
    {
        if (otherStack.isMatching || isMatching)
        {
            return;
        }
        StartCoroutine(TransferCoroutine(otherStack));
    }

    IEnumerator TransferCoroutine(Stack otherStack)
    { 
        otherStack.isMatching = true;
        isMatching = true; 
        Color color = stackTilesObjects[stackTilesObjects.Count - 1].color;

        for (int i = stackTilesObjects.Count - 1; i >= 0; i--)
        { 
            if (stackTilesObjects[i].color == color)
            {
                otherStack.AddStackTileToStack(stackTilesObjects[i]);
                stackTilesObjects.RemoveAt(i);
                yield return new WaitForSeconds(0.1f);

            } 
            else
            {
                break;
            } 
           
        }
        yield return new WaitForSeconds(0.3f);
        isMatching = false;
        otherStack.isMatching = false;
       LevelManager.instance.CheckBoomAll();
        LevelManager.onCheckNeeded.Invoke();
        CheckEmpty();
    }
   
    public void CheckBoom()
    {
        if (stackTilesObjects.Count == 0)
        {
            return;
        }
    
        Color color = stackTilesObjects[stackTilesObjects.Count - 1].color;
        List<StackObject> stackObjects = new List<StackObject>();

        for (int i = stackTilesObjects.Count - 1; i >= 0; i--)
        { 
            if (stackTilesObjects[i].color == color)
            {
                stackObjects.Add(stackTilesObjects[i]);
            }
        }

        if (stackObjects.Count >= 10)
        {
            StartCoroutine(PerformActionsCoroutine(stackObjects));
        }
    }

    IEnumerator PerformActionsCoroutine(List<StackObject> stackObjects)
    {
        yield return new WaitForSeconds(0.2f);
        int count = 0;
        bool isStar = false;

        foreach (var stackObject in stackObjects)
        {
            count++;
            ScoreManager.instance.ChangeScore(1);
            stackTilesObjects.Remove(stackObject);
            stackObject.transform.DOScale(Vector3.zero, 0.3f)
                .OnComplete(() =>
                {
                    if (count == stackObjects.Count && !isStar)
                    { 
                        isStar = true;
                        UiController.instance.ProgressStar(stackObject.transform.position);
                    }
                    SimpleGameObjectPool.instance.ReturnObject(stackObject.gameObject);
                });

            yield return new WaitForSeconds(0.1f);
        }

        CheckEmpty();
        LevelManager.onCheckNeeded.Invoke();
    }

    public void CheckEmpty()
    {
        if (stackTilesObjects.Count == 0)
        {  
            currentGrid.isEmpty = true;
            currentGrid.CurrentStack = null;
            Destroy(gameObject);
        }
        else return;
    }
}
