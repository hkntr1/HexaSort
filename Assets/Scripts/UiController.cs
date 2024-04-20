using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class UiController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Slider progressSlider;
    [SerializeField] Image winPanel,failPanel;
    
public void Init(int maxProgress) {
    scoreText.text = ScoreManager.instance.score+"/"+maxProgress;
    progressSlider.value = 0;
    progressSlider.maxValue = maxProgress;
}
public void UpdateProgress(int score) {

    progressSlider.DOValue(score, 0.5f).OnUpdate(() => {
      if(CheckWin())
      {
            progressSlider.DOKill();
      }
      scoreText.text = progressSlider.value+"/"+progressSlider.maxValue;
    });
   
}
public void ResetScore() {
    scoreText.text = "0"+"/"+GameManager.instance.levels[GameManager.instance.currentLevel].maxProgress ;
    progressSlider.value = 0;
}
public bool CheckWin() {
    if(progressSlider.value >= progressSlider.maxValue) {
        winPanel.gameObject.SetActive(true);
        winPanel.DOFade(1f, 0.5f).OnComplete(() =>
         {
            LevelManager.instance.ClearStacks();
            GameManager.instance.currentLevelData.gameObject.SetActive(false);
        });
        return true;
    }
    else {
        return false;
    }
}
public void FailScreen() {
    failPanel.gameObject.SetActive(true);
    failPanel.DOFade(1f, 0.5f);
}
public void NextLevel() {
    GameManager.onLevelChange?.Invoke();
    winPanel.gameObject.SetActive(false);
}
}