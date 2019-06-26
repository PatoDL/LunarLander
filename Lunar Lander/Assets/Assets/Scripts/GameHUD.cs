using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviourSingleton<GameHUD>
{
    public GameObject rocket;

    public Text scoreText;
    public Text timeText;
    public Text fuelText;
    public Text altitudeText;
    public Text xVelText;
    public Text yVelText;

    public Text landResult;
    public GameObject landResultPanel;
    public Button keepPlayingButton;
    public Text keepPlayingText;
    public Text resultScoreText;

    public Button PauseButton;

    RocketBehaviour r;

    public delegate void OnReturnToMenu();
    public static OnReturnToMenu ReturnToMenu;

    void Start()
    {
        r = rocket.GetComponent<RocketBehaviour>();
        ReturnToMenu += LevelManager.Get().GoToMenu;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + GameManager.Get().GetScore();
        timeText.text = "Time: " + GameManager.Get().GetTime();
        fuelText.text = "Fuel: " + (int)r.fuel;
        altitudeText.text = "Altitude: " + (int)r.altitude;
        xVelText.text = "xVel: " + (int)(r.rig.velocity.x*10);
        yVelText.text = "yVel: " + (int)(r.rig.velocity.y*10);
    }

    public void GoToMenu()
    {
        ReturnToMenu();
    }

    public void Pause()
    {
        GameManager.PauseGame();
    }

    public void Resume()
    {
        GameManager.ResumeGame();
    }

    public void ShowResult(bool lose)
    {
        landResultPanel.SetActive(true);
        if (lose)
        {
            landResult.text = "Landing Failed";
            keepPlayingText.text = "Try Again";
            resultScoreText.text = "Final Score: " + GameManager.Get().finalScore;
        }
        else
        {
            landResult.text = "Successful Landing";
            keepPlayingText.text = "Next Level";
            resultScoreText.text = "Actual Score: " + GameManager.Get().GetScore();
        }
    }
}
