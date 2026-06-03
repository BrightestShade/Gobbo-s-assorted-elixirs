using UnityEngine;
using UnityEngine.UI;
using System;

public class PatienceBarRequest : MonoBehaviour
{
    public Image fillImage;
    public GameObject uiRoot;

    public float maxPatience = 20f;
    private float currentPatience;

    private bool running = false;

    public event Action OnTimerExpired;
    private Color baseColor;

    void Awake()
    {
        ColorUtility.TryParseHtmlString("#E96C00", out baseColor);
        uiRoot.SetActive(false);
    }
   

    public void StartTimer(float duration)
    {
        maxPatience = duration;
        currentPatience = duration;

        fillImage.fillAmount = 1f;
        fillImage.color = baseColor;

        uiRoot.SetActive(true); 

        running = true;
    }

    public void StopTimer()
    {
        running = false;

        uiRoot.SetActive(false);

        OnTimerExpired = null;
    }

    void Update()
    {
        if (!running) return;

        currentPatience -= Time.deltaTime;

        float newFill = currentPatience / maxPatience;
        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, newFill, Time.deltaTime * 8f);

        if (currentPatience <= 0f)
        {
            currentPatience = 0f;
            running = false;

            uiRoot.SetActive(false); 

            OnTimerExpired?.Invoke();
        }

        if (fillImage.fillAmount < 0.3f)
        {
            fillImage.color = Color.red;
        }
        else
        {
            fillImage.color = baseColor; 
        }
    }
}