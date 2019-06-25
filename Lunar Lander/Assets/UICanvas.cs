using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    public GameObject uiRocket;

    public Button playButton;

    public GameObject firstSplashScreen;

    public Button quitButton;
    
    void Start()
    {
        firstSplashScreen.GetComponent<Image>().material.color = Color.white;
        FadeLogo();
        playButton.onClick.AddListener(FadeLogo);
    }

    void Update()
    {
        
    }

    public void MoveRocketToQuitButton()
    {
        Vector3 newPos = quitButton.transform.position;
        uiRocket.transform.position = new Vector3(newPos.x, newPos.y + 10f);
    }

    public void MoveRocketToPlayButton()
    {
        Debug.Log(playButton.transform.position);
        Vector3 newPos = Camera.main.ScreenToWorldPoint(playButton.transform.position);
       
        uiRocket.transform.position = new Vector3(newPos.x, newPos.y + 10f);
    }

    public void FadeLogo()
    {
        StartCoroutine(Fade(firstSplashScreen));
    }

    IEnumerator RocketStartAnimation()
    {
        float yScale = uiRocket.transform.localScale.y;
        Bounds bounds = CameraUtils.OrthographicBounds();
        bool rocketOutOfView = uiRocket.transform.position.x < bounds.max.y + yScale;
        while (rocketOutOfView)
        {
            uiRocket.transform.position += Vector3.up * Time.deltaTime * 5f;
            yield return null;
        }
    }

    IEnumerator Fade(GameObject g)
    {
        yield return new WaitForSeconds(2);
        float t = 1;
        Material mat = g.GetComponent<Image>().material;
        Color c = mat.color;

        while (t > 0)
        {
            t -= Time.deltaTime;
            c.a = t;
            mat.color = c;
            if(t<=0)
            {
                g.SetActive(false);
                //StartCoroutine(RocketStartAnimation());
            }
            yield return null;
        }
    }
}
