using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    int level;
    int score;
    float timer;


    public GameObject rocket;

    void Start()
    {
        score = 0;
        level = 1;
        RocketBehaviour.RocketWin += AddScore;
        RocketBehaviour.RocketWin += PassLevel;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
    }

    void AddScore()
    {
        score += 75;
    }

    void PassLevel()
    {
        level++;
        Debug.Log("Successfully landed");
    }

    public int GetScore()
    {
        return score;
    }

    public int GetTime()
    {
        return (int)timer;
    }
}
