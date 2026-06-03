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

    [Header("Arrival Patience")]
    public ArrivalPatienceBar arrivalBarUI;

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

        transform.LookAt(targetPoint);

        StartArrivalBar();

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

        StartCoroutine(StartTimerAfterText());
    }

    void OnPotionFinished()
    {
        potionReady = true;
    }

    void CompleteInteraction()
    {
        if (cauldron.brewedPotion != requestedPotion)
        {
            requestBarUI.StopTimer();
            requestBarUI.OnTimerExpired -= HandleTimerFail;

            UIManager.Instance.ShowMessage("Thank you, farewell");

            float loseDelay = Random.Range(10f, 30f);
            GameOverManager.Instance.QueueLose(loseDelay);

            isLeaving = true;

            StartCoroutine(DelayedLeave(2f));
            return;
        }

        requestBarUI.StopTimer();
        requestBarUI.OnTimerExpired -= HandleTimerFail;

        ScoreManager.Instance.AddScore();

        UIManager.Instance.ShowMessage("Thank you, farewell.");

        isLeaving = true;

        StartCoroutine(DelayedLeave(2f));
    }

    IEnumerator StartTimerAfterText()
    {
        yield return new WaitForSeconds(0.5f);

        requestBarUI.OnTimerExpired += HandleTimerFail;

     
        requestBarUI.StartTimer();
    }

    IEnumerator DelayedLeave(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Leave());
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

    public bool IsPotionReady() => potionReady;
    public bool HasGivenOrder() => hasGivenOrder;

    void HandleTimerFail()
    {
        if (isLeaving) return;

        Debug.Log("Request timer ended");
        SceneManager.LoadSceneAsync(3);
    }

    void StartArrivalBar()
    {
        arrivalBarUI.OnExpired += HandleArrivalFail;
        arrivalBarUI.StartBar();
    }

    void HandleArrivalFail()
    {
        if (isLeaving) return;

        Debug.Log("Adventurer ignored");

        arrivalBarUI.OnExpired -= HandleArrivalFail;

        ScoreManager.Instance.RemoveScore(10);

        UIManager.Instance.ShowMessage(
            "I suppose i'll just take some gold and leave..."
        );

        isLeaving = true;

        StartCoroutine(DelayedLeave(2f));
    }
}