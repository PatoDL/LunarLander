using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    int maxAmountOfLevels = 2;
    int actualLevel;
    int nextLevel;
    int prevLevel;
    string actualLevelName;

    LevelData actualLevelData;

    void Start()
    {
        actualLevelData = GameObject.Find("LevelData").GetComponent<LevelData>();
        actualLevel = actualLevelData.level;
        nextLevel = actualLevelData.nextLevel;
        prevLevel = actualLevelData.previousLevel;
        actualLevelName = actualLevelData.name;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
