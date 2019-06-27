using UnityEngine.UI;

public class UILoadingScreen : MonoBehaviourSingleton<UILoadingScreen>
{
    public Text percentageText;
    public Text loadingText;

    public override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public void SetVisible(bool show)
    {
        gameObject.SetActive(show);
    }

    public void Update()
    {
        loadingText.text = "Loading lv." + GameManager.Get().level;
        int loadingVal = (int) (LoaderManager.Get().loadingProgress*100);
        percentageText.text = "Loading " + loadingVal + "%";
        if (LoaderManager.Get().loadingProgress >= 1)
            SetVisible(false);
    }

}