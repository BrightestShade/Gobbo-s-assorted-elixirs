using UnityEngine;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    public TMP_Text highScoreText;

    void Start()
    {
        highScoreText.text =
            "Highest Earnings: " + PlayerPrefs.GetInt("HighScore", 0);
    }
}