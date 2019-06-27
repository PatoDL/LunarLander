using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public int level;
    int score;
    float timer;
    public static GameObject rocket;
    public static GameObject terrainGenerator;

    public delegate void OnPause();
    public static OnPause PauseGame;

    public delegate void OnResume();
    public static OnResume ResumeGame;

    Vector3 cameraStartPos;

    void Start()
    {
        score = 0;
        level = 1;
        cameraStartPos = Camera.main.transform.position;
        RocketBehaviour.RocketWin += AddScore;
        RocketBehaviour.RocketWin += PassLevel;
        RocketBehaviour.RocketDeath += RestartLevel;
        RocketBehaviour.RocketDeath += RestartScore;
        GameHUD.RePlay += Play;
    }
    
    static public void ReloadReferences()
    {
        rocket = GameObject.Find("Rocket");
        terrainGenerator = GameObject.Find("TerrainGenerator");
    }

    public void ZoomCamera(bool zoom)
    {
        if (zoom)
        {
            Camera.main.orthographicSize = 1.37f;
            Vector3 cameraZoomPosition = new Vector3(rocket.transform.position.x, rocket.transform.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = cameraZoomPosition;
        }
        else
        {
            Camera.main.orthographicSize = 5f;
            Camera.main.transform.position = cameraStartPos;
        }
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
    }

    void RestartLevel()
    {
        level = 1;
    }

    public float finalScore;

    void RestartScore()
    {
        finalScore = score;
        score = 0;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetTime()
    {
        return (int)timer;
    }

    public void Play()
    {
        ZoomCamera(false);
        rocket.GetComponent<RocketBehaviour>().RestartRocket();
        terrainGenerator.GetComponent<TerrainGenerator>().StartTerrain();
    }
}
