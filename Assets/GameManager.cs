using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{   public static Action onLevelChange;
    public List<LevelData> levels;
    public int currentLevel;
    public static GameManager instance;
    private void Awake() {
        instance = this;
        Application.targetFrameRate = 60;
        onLevelChange += NextLevel;
    }
    public void NextLevel()
    {
       //Destroy(levels[currentLevel]);
        currentLevel++;
        ScoreManager.instance.ResetScore();
        WaveController.instance.ResetWave();
    }
}
