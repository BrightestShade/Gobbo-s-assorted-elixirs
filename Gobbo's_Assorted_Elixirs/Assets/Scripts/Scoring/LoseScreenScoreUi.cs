using UnityEngine;
using TMPro;

public class LoseScreenScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    void Start()
    {
        ScoreManager.Instance.SaveHighScore();

        scoreText.text =
            "Earnings: " + ScoreManager.Instance.CurrentScore;

        highScoreText.text =
            "Highest Earnings: " + ScoreManager.Instance.HighScore;
    }
}