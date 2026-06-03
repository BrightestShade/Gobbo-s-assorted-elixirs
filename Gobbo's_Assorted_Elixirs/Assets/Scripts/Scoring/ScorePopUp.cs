using UnityEngine;
using TMPro;

public class ScorePopUp : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float lifeTime = 1f;

    private TextMeshProUGUI textMesh;
    private RectTransform rectTransform;

    private Color textColor;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        textColor = textMesh.color;

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move upward in UI space
        rectTransform.anchoredPosition +=
            Vector2.up * moveSpeed * Time.deltaTime;

        // Fade out
        textColor.a -= Time.deltaTime / lifeTime;

        textMesh.color = textColor;
    }

    public void SetText(string text)
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshProUGUI>();

        textMesh.text = text;
    }
}