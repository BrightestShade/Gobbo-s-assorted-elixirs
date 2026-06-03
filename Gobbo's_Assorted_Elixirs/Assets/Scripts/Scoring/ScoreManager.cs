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
    private int highScore = 0;

    public int CurrentScore => currentScore;
    public int HighScore => highScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void SaveHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
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

    void SpawnNegativeFloatingText(int amount)
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
            floatingText.SetText("-" + amount);
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Earnings: " + currentScore;
        }
    }

    public void RemoveScore(int amount)
    {
        currentScore -= amount;

        // prevents score going below 0. Not needed but will keep incase needed later.
        //if (currentScore < 0)
           // currentScore = 0;

        UpdateScoreUI();

        SpawnNegativeFloatingText(amount);
    }
}