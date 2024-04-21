using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManager : MonoBehaviour
{
    private const string PlayerPrefKey = "LastSceneIndex";
    
    public void ContinueGame()
    {
        PlayerPrefs.SetInt("Continue", 1);
        LoadScene(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("Continue", 0);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void SaveGame(GameManager gameManager)
    {
        PlayerPrefs.SetInt(PlayerPrefKey, gameManager.currentLevel);
        PlayerPrefs.Save();
    }
    private void LoadScene(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void GoMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    
}
