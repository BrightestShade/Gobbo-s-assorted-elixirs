using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    private bool loseQueued = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void QueueLose(float delay)
    {
        if (!loseQueued)
        {
            loseQueued = true;

            StartCoroutine(DelayedLose(delay));
        }
    }

    IEnumerator DelayedLose(float delay)
    {
        Debug.Log("Lose timer started");

        yield return new WaitForSecondsRealtime(delay);

        Debug.Log("Loading lose scene");

        SceneManager.LoadSceneAsync(2);
    }
}