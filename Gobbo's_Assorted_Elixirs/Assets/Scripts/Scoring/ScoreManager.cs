using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Score Settings")]
    public int pointsPerPotion = 100;

    [Header("World Text")]
    public TextMeshPro scoreText;

    [Header("Floating Popup")]
    public GameObject floatingTextPrefab;

    public Transform popupSpawnPoint;

    private int currentScore = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore()
    {
        currentScore += pointsPerPotion;

        UpdateScoreUI();

        SpawnFloatingText(pointsPerPotion);
    }

    void SpawnFloatingText(int amount)
    {
        if (floatingTextPrefab == null || popupSpawnPoint == null)
            return;

        GameObject popup = Instantiate(
            floatingTextPrefab,
            popupSpawnPoint.position,
            Quaternion.identity
        );

        ScorePopUp floatingText = popup.GetComponent<ScorePopUp>();

        if (floatingText != null)
        {
            floatingText.SetText("+" + amount);
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Earnings" + currentScore;
        }
    }
}