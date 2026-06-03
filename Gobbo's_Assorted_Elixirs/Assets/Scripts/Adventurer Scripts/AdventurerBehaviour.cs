using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AdventurerBehaviour : MonoBehaviour
{
    public Transform targetPoint;
    public float speed = 2f;

    public PotionRecipe requestedPotion;
    public PotionSelector potionSelector;

    private AdventurerSpawner spawner;

    public Transform exitPoint;
    public Transform lookPoint;

    private CauldronIngredientChecker cauldron;
    private bool isLeaving = false;

    public AdventurerInteraction orderTrigger;

    private bool hasGivenOrder = false;
    private bool potionReady = false;

    private static bool firstAdventurerSpawned = false;

    [Header("Request Patience")]
    public PatienceBarRequest requestBarUI;
    public float requestTimeLimit = 90f;

    [Header("Arrival Patience")]
    public ArrivalPatienceBar arrivalBarUI;
    public float arrivalTimeLimit = 10f;

    void Start()
    {
        if (requestBarUI == null)
            requestBarUI = FindObjectOfType<PatienceBarRequest>();

        if (arrivalBarUI == null)
            arrivalBarUI = FindObjectOfType<ArrivalPatienceBar>();

        spawner = FindObjectOfType<AdventurerSpawner>();
        cauldron = FindObjectOfType<CauldronIngredientChecker>();

        StartCoroutine(MoveAndWait());
    }

    IEnumerator MoveAndWait()
    {
        // Move to counter
        while (Vector3.Distance(transform.position, targetPoint.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPoint.position,
                speed * Time.deltaTime
            );

            Vector3 direction = (targetPoint.position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    lookRotation,
                    Time.deltaTime * 5f
                );
            }

            yield return null;
        }

        // Face counter initially
        transform.LookAt(targetPoint);

        // Start arrival patience
        StartArrivalBar();

        // Setup interaction
        if (orderTrigger != null)
        {
            orderTrigger.SetAdventurer(this);

            if (!firstAdventurerSpawned)
            {
                UIManager.Instance.ShowMessage(
                    "Hello? Over here by the window."
                );

                firstAdventurerSpawned = true;
            }
        }

        // Idle look behaviour
        while (!isLeaving)
        {
            if (lookPoint != null)
            {
                Vector3 direction =
                    (lookPoint.position - transform.position).normalized;

                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation =
                        Quaternion.LookRotation(direction);

                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        lookRotation,
                        Time.deltaTime * 2f
                    );
                }
            }

            yield return null;
        }
    }

    public void Interact()
    {
        // Stop arrival patience when player talks
        arrivalBarUI.StopBar();
        arrivalBarUI.OnExpired -= HandleArrivalFail;

        if (!hasGivenOrder)
        {
            GiveOrder();
        }
        else if (potionReady && !isLeaving)
        {
            CompleteInteraction();
        }
    }

    void GiveOrder()
    {
        hasGivenOrder = true;

        if (requestedPotion == null)
        {
            Debug.LogError("No potion assigned!");
            return;
        }

        switch (requestedPotion.name)
        {
            case "HealthPotion":
                UIManager.Instance.ShowMessage(
                    "I am going to fight something dangerous, do you have something to help me stay alive?"
                );
                break;

            case "InvisPotion":
                UIManager.Instance.ShowMessage(
                    "I need to sneak past something... got anything useful?"
                );
                break;
        }

        cauldron.OnPotionComplete += OnPotionFinished;

        // Start request patience after dialogue
        StartCoroutine(StartTimerAfterText());
    }

    void OnPotionFinished()
    {
        potionReady = true;
    }

    void CompleteInteraction()
    {
        // Wrong potion
        if (cauldron.brewedPotion != requestedPotion)
        {
            requestBarUI.StopTimer();
            requestBarUI.OnTimerExpired -= HandleTimerFail;

            UIManager.Instance.ShowMessage("Thank you, farewell");

            Debug.Log("GameOver");

            float loseDelay = Random.Range(10f, 30f);

            GameOverManager.Instance.QueueLose(loseDelay);

            isLeaving = true;

            StartCoroutine(DelayedLeave(2f));

            return;
        }

        // Correct potion
        requestBarUI.StopTimer();
        requestBarUI.OnTimerExpired -= HandleTimerFail;

        ScoreManager.Instance.AddScore();

        UIManager.Instance.ShowMessage("Thank you, farewell.");

        isLeaving = true;

        StartCoroutine(DelayedLeave(2f));
    }

    IEnumerator DelayedLeave(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(Leave());
    }

    IEnumerator StartTimerAfterText()
    {
        yield return new WaitForSeconds(0.5f);

        requestBarUI.OnTimerExpired += HandleTimerFail;
        requestBarUI.StartTimer(requestTimeLimit);
    }

    IEnumerator Leave()
    {
        cauldron.OnPotionComplete -= OnPotionFinished;

        while (Vector3.Distance(transform.position, exitPoint.position) > 0.1f)
        {
            Vector3 direction =
                (exitPoint.position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation =
                    Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    lookRotation,
                    Time.deltaTime * 5f
                );
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                exitPoint.position,
                speed * Time.deltaTime
            );

            yield return null;
        }

        spawner.AdventurerFinished();

        Destroy(gameObject);
    }

    public bool IsPotionReady()
    {
        return potionReady;
    }

    public bool HasGivenOrder()
    {
        return hasGivenOrder;
    }

    void HandleTimerFail()
    {
        if (isLeaving) return;

        Debug.Log("Request timer ended");

        SceneManager.LoadSceneAsync(3);
    }

    void StartArrivalBar()
    {
        arrivalBarUI.OnExpired += HandleArrivalFail;
        arrivalBarUI.StartBar(arrivalTimeLimit);
    }

    void HandleArrivalFail()
    {
        if (isLeaving) return;

        Debug.Log("Adventurer ignored");

        arrivalBarUI.OnExpired -= HandleArrivalFail;

        ScoreManager.Instance.RemoveScore(10);

        UIManager.Instance.ShowMessage
        (
            "I suppose i'll just take some gold and leave..."
        );

        isLeaving = true;

        StartCoroutine(DelayedLeave(2f));
    }
}