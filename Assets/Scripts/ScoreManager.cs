using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{ 
    public int score;
    public static ScoreManager instance;
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void ChangeScore(int value)
    {
        score += value;
    }
}
