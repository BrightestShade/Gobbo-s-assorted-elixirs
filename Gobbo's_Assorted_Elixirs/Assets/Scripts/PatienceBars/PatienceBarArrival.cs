using UnityEngine;
using UnityEngine.UI;
using System;

public class ArrivalPatienceBar : MonoBehaviour
{
    public Image fillImage;
    public GameObject uiRoot;

    public float maxPatience = 10f;
    private float currentPatience;

    private bool running;

    public event Action OnExpired;

    private Color baseColor;

    void Awake()
    {
        ColorUtility.TryParseHtmlString("#E96C00", out baseColor);
        uiRoot.SetActive(false);
    }

    public void StartBar(float duration)
    {
        maxPatience = duration;
        currentPatience = duration;

        fillImage.fillAmount = 1f;
        fillImage.color = baseColor;

        uiRoot.SetActive(true);
        running = true;
    }

    public void StopBar()
    {
        running = false;
        uiRoot.SetActive(false);
        OnExpired = null;
    }

    void Update()
    {
        if (!running) return;

        currentPatience -= Time.deltaTime;

        float fill = currentPatience / maxPatience;
        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, fill, Time.deltaTime * 8f);

        if (currentPatience <= 0f)
        {
            running = false;
            uiRoot.SetActive(false);
            OnExpired?.Invoke();
        }

        if (fillImage.fillAmount < 0.3f)
            fillImage.color = Color.red;
        else
            fillImage.color = baseColor;
    }
}