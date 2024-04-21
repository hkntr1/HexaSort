using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static Action onLevelChange;
    public List<LevelData> levels;
    public Transform enviromentParent;
    public int currentLevel;
    public LevelData currentLevelData;
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        onLevelChange += () =>
        {
            NextLevel();
        };
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("Continue", 0) == 0)
        {
            currentLevel = 0;
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        }

        currentLevelData = Instantiate(levels[currentLevel]);
        currentLevelData.transform.parent = enviromentParent;
    }

    public void NextLevel()
    {
        int thisLevel = currentLevel;
        foreach (var gridManager in LevelManager.instance.gridManagers)
        {
            if (gridManager.CurrentStack != null)
            {
                Destroy(gridManager.CurrentStack);
            }
        }
        
        Destroy(currentLevelData.gameObject);
        currentLevel++;
        currentLevelData = Instantiate(levels[currentLevel]);
        currentLevelData.transform.parent = enviromentParent;
        ScoreManager.instance.ResetScore();
        WaveController.instance.ResetWave();
        LevelManager.instance.Init();
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
    }
}
