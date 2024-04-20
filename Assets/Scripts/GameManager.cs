using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{   public static Action onLevelChange;
    public List<LevelData> levels;
    public int currentLevel;
    public LevelData currentLevelData;
    public static GameManager instance;
    private void Awake() {
        instance = this;
        Application.targetFrameRate = 60;
        onLevelChange += NextLevel;
    }
    private void Start() {
        currentLevelData =  Instantiate(GameManager.instance.levels[GameManager.instance.currentLevel]);
    }
    public void NextLevel()
    {
        foreach (var gridManager in LevelManager.instance.gridManagers)
        {
            if(gridManager.CurrentStack!=null)
            {
               Destroy(gridManager.CurrentStack);
            }
            
        }
        Destroy(currentLevelData.gameObject);
        currentLevel++;
        currentLevelData =  Instantiate(levels[currentLevel]);
        ScoreManager.instance.ResetScore();
        WaveController.instance.ResetWave();
        LevelManager.instance.Init();
    }
  
}
