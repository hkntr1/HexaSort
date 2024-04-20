using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   public static Action<int> onScoreUpdate;
    [SerializeField] int maxProgress;
   
    private void Awake() {
        Application.targetFrameRate = 60;
       
    }
}
