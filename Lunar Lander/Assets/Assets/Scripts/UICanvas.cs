using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    public GameObject uiRocket;

    public Button playButton;

    public GameObject fadePanel;

    public Button quitButton;

    public GameObject title;

    float buttonYOffset = -1f;

    Bounds bounds;

    Vector3 titleTruePos;

    public delegate void OnGameQuitted();
    public static OnGameQuitted Quit;

    void Start()
    {
        bounds = CameraUtils.OrthographicBounds();
        Vector3 pbPos = playButton.transform.position;
        uiRocket.transform.position = new Vector3(pbPos.x, pbPos.y + buttonYOffset);
        fadePanel.GetComponent<Image>().material.color = Color.white;
        FadeLogo();
        playButton.onClick.AddListener(FadeLogo);
        titleTruePos = title.transform.position;
        title.GetComponent<Image>().material.color = new Color(1, 1, 1, 0);
        title.transform.position = bounds.max / 2;
    }

    public void QuitGame()
    {
        Quit();
    }

    public void MoveRocketToQuitButton()
    {
        Vector3 newPos = quitButton.transform.position;
        uiRocket.transform.position = new Vector3(newPos.x, newPos.y +buttonYOffset);
    }

    public void MoveRocketToPlayButton()
    {
        Vector3 newPos = playButton.transform.position;
        uiRocket.transform.position = new Vector3(newPos.x,newPos.y+buttonYOffset);
    }

    public void FadeLogo()
    {
        GameObject logo = fadePanel.transform.Find("CachuflitoGamesLogo").gameObject;
        logo.GetComponent<Image>().material.color = Color.white;
        StartCoroutine(Fade(logo));
    }

    IEnumerator RocketStartAnimation()
    {
        float yScale = uiRocket.transform.localScale.y;
        
        bool rocketOutOfView = uiRocket.transform.position.y > bounds.max.y + yScale;
        while (!rocketOutOfView)
        {
            rocketOutOfView = uiRocket.transform.position.y > bounds.max.y + yScale;
            uiRocket.transform.position += Vector3.up * Time.deltaTime * 5f;
            if (rocketOutOfView)
            {
                LevelManager.Get().GoToNextLevel();
            }
            yield return null;
        }
    }

    public void GameStart()
    {
        StartCoroutine(RocketStartAnimation());
    }

    IEnumerator UnFade(GameObject g)
    {
        yield return new WaitForSeconds(2);
        float t = 0;
        Material mat = g.GetComponent<Image>().material;
        Color c = mat.color;

        while (t < 1)
        {
            t += Time.deltaTime;
            c.a = t;
            mat.color = c;
            if (t >= 1)
            {
                yield return new WaitForSeconds(1);
                while(title.transform.position.y<titleTruePos.y)
                {
                    title.transform.position += Vector3.up * Time.deltaTime;
                    yield return null; 
                }
                StartCoroutine(Fade(fadePanel));
            }
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
                if (g.name == "CachuflitoGamesLogo")
                {
                    yield return new WaitForSeconds(1);
                    StartCoroutine(UnFade(title));
                }
                else if(g.name=="FadePanel")
                {
                    uiRocket.GetComponent<ParticleSystem>().Play();
                }
                g.SetActive(false);
            }
            yield return null;
        }
    }
}
