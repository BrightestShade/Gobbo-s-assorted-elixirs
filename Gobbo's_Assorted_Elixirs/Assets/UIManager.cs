using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TMP_Text messageText;
    public float typeDuration = 2f;   
    public float fadeDuration = 2f;   

    private Coroutine currentCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowMessage(string message)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(ShowMessageRoutine(message));
    }

    private IEnumerator ShowMessageRoutine(string message)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = "";

        // Typewriter effect over typeDuration
        float timer = 0f;
        int totalChars = message.Length;
        while (timer < typeDuration)
        {
            timer += Time.deltaTime;
            int charsToShow = Mathf.Clamp(Mathf.FloorToInt((timer / typeDuration) * totalChars), 0, totalChars);
            messageText.text = message.Substring(0, charsToShow);
            yield return null;
        }

        // Ensure full text is visible
        messageText.text = message;

        // Wait a tiny bit so user sees the full text before fading
        yield return new WaitForSeconds(0.1f);

        // Fade out over fadeDuration
        timer = 0f;
        Color originalColor = messageText.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        messageText.gameObject.SetActive(false);

        // Reset alpha for next message
        messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        currentCoroutine = null;
    }
}