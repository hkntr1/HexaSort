using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{ 
    public int score;
    public int maxProgress;
    public static ScoreManager instance;
    private UiController uiController;
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
        uiController = FindObjectOfType<UiController>();
    }
    private void Start() {
        uiController.Init(maxProgress);
    }
    public void ChangeScore(int value)
    {
        score += value;
        uiController.UpdateProgress(score);}
}
