using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    void Update()
    {
        // ESC key to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f; // freezes gameplay
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0);
    }
}