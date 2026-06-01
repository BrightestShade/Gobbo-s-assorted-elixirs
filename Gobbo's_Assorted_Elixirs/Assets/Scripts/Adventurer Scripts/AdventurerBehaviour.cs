using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdventurerBehaviour : MonoBehaviour
{
    public Transform targetPoint; // target point for movement 
    public float speed = 2f; // movement speed
    
    
    public PotionRecipe requestedPotion; 
    public PotionSelector potionSelector;

    private AdventurerSpawner spawner;
    public Transform exitPoint; // despawn point for adventurer

    public Transform lookPoint; // where the adventurer looks

    private CauldronIngredientChecker cauldron; // reference to cauldron checker script
    private bool isLeaving = false; 


    public AdventurerInteraction orderTrigger;
    private bool hasGivenOrder = false;
    private bool potionReady = false;

    

    private static bool firstAdventurerSpawned = false;

    public RequestTimer timerUI;
    public float requestTimeLimit = 90f;
    void Start()
    {
        if (timerUI == null)
            timerUI = FindObjectOfType<RequestTimer>();

        spawner = FindObjectOfType<AdventurerSpawner>();
        cauldron = FindObjectOfType<CauldronIngredientChecker>(); //checks the Adventurer spawner and cauldronIngredient checker objects are in the scene

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
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            yield return null;
        }

      
        transform.LookAt(targetPoint);

        if (orderTrigger != null)
        {
            orderTrigger.SetAdventurer(this);

            if (!firstAdventurerSpawned)
            {
                UIManager.Instance.ShowMessage("Hello? Over here by the window.");
                firstAdventurerSpawned = true;
            }
        }



        while (!isLeaving)
        {
            if (lookPoint != null)
            {
                Vector3 direction = (lookPoint.position - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);
                }
            }
            yield return null;
        }
    }
    public void Interact()
    {
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
                UIManager.Instance.ShowMessage("I am going to fight something dangerous, do you have something to help me stay alive?");
                break;

            case "InvisPotion":
                UIManager.Instance.ShowMessage("I need to sneak past something... got anything useful?");
                break;
        }

        cauldron.OnPotionComplete += OnPotionFinished;

        
        StartCoroutine(StartTimerAfterText()); // Starts the Request timer after the adventurer finishes speaking
    }

    void OnPotionFinished()
    {
        potionReady = true;

    }
    void CompleteInteraction()
    {
        if (cauldron.brewedPotion != requestedPotion)
        {
            timerUI.StopTimer();
            timerUI.OnTimerExpired -= HandleTimerFail;

            UIManager.Instance.ShowMessage("Thank you, farewell");

            Debug.Log("GameOver");

            float loseDelay = Random.Range(10f, 30f); // Configurable, idea behind the delay before the lose screen is to throw off the player. E.g. They wont immediately know which potion they got wrong. This would help to increase the replayability of the game if there was more potion recipes.

            GameOverManager.Instance.QueueLose(loseDelay);

            isLeaving = true;

            StartCoroutine(DelayedLeave(2f));

            return;
        }
        timerUI.StopTimer();
        timerUI.OnTimerExpired -= HandleTimerFail;
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
        yield return new WaitForSeconds(0.5f); // adjust if needed

        timerUI.OnTimerExpired += HandleTimerFail;
        timerUI.StartTimer(requestTimeLimit);
    }

    IEnumerator Leave()
    {
        cauldron.OnPotionComplete -= OnPotionFinished;
        

        while (Vector3.Distance(transform.position, exitPoint.position) > 0.1f)
        {
            
            Vector3 direction = (exitPoint.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
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

        Debug.Log("Timer ended");

        SceneManager.LoadSceneAsync(3);
    }
}