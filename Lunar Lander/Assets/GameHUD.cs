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

    RocketBehaviour r;
    void Start()
    {
        r = rocket.GetComponent<RocketBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + GameManager.Get().GetScore();
        timeText.text = "Time: " + GameManager.Get().GetTime();
        fuelText.text = "Fuel: " + r.fuel;
        altitudeText.text = "Altitude: " + (int)r.altitude;
        xVelText.text = "xVel: " + (int)r.rig.velocity.x*10;
        yVelText.text = "yVel: " + (int)r.rig.velocity.y*10;
    }
}
