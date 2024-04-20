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
    [SerializeField] Image winPanel;
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
        winPanel.DOFade(0.8f, 0.5f);
        return true;
    }
    else {
        return false;
    }
}
public void NextLevel() {
    GameManager.onLevelChange?.Invoke();
    winPanel.DOFade(0, 0.5f).OnComplete(() => {
        winPanel.gameObject.SetActive(false);
    });
}
}
