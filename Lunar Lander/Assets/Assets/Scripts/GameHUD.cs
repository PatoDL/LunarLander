using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
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

    static GameHUD instance;

    static public GameHUD Get()
    {
        return instance;
    }

    void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    GameManager gm;

    LevelManager lm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        r = rocket.GetComponent<RocketBehaviour>();
        ReturnToMenu += lm.GoToMenu;
        RocketBehaviour.ShowResult += ShowResult;
    }

    // Update is called once per frame
    void Update()
    {
        Start();
        scoreText.text = "Score: " + gm.GetScore();
        timeText.text = "Time: " + gm.GetTime();
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
            resultScoreText.text = "Final Score: " + gm.finalScore;
        }
        else
        {
            landResult.text = "Successful Landing";
            keepPlayingText.text = "Next Level";
            resultScoreText.text = "Actual Score: " + gm.GetScore();
        }
    }
}
