using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    int actualLevel;
    int nextLevel;
    int prevLevel;
    string actualLevelName;
    int actualLevelThatComesFrom;
    LevelData actualLevelData;

    int savedLevelThatComesFrom;

    void Start()
    {
        StartNewLevel();
        GameHUD.ReturnToMenu = GoToMenu;
        UICanvas.Quit = QuitGame;
    }

    void StartNewLevel()
    {
        actualLevelData = GameObject.Find("LevelData").GetComponent<LevelData>();
        actualLevel = actualLevelData.level;
        nextLevel = actualLevelData.nextLevel;
        prevLevel = actualLevelData.previousLevel;
        actualLevelName = actualLevelData.name;
        actualLevelThatComesFrom = actualLevelData.levelThatComesFrom;
    }

    public int GetActualLevel()
    {
        return actualLevel;
    }

    public int GetLevelThatComesFrom()
    {
        return actualLevelThatComesFrom;
    }

    void Update()
    {
        StartNewLevel();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
        actualLevelData.levelThatComesFrom = savedLevelThatComesFrom;
        StartNewLevel();
        savedLevelThatComesFrom = actualLevel;
    }

    public void GoToNextLevel()
    {
        LoaderManager.Get().uiLoadingScreen.SetActive(true);
        LoaderManager.Get().fakeLoad = true;
        LoaderManager.Get().LoadScene(nextLevel);
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
