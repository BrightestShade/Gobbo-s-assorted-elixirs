using UnityEngine;
using System.Collections;
using UnityEngine.UI;
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

    

    void Start()
    {
        spawner = FindObjectOfType<AdventurerSpawner>(); //checks the Adventurer spawner and cauldronIngredient checker objects are in the scene
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

        potionSelector.SetPotion(requestedPotion);

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
    }

    void OnPotionFinished()
    {
        potionReady = true;

    }
    void CompleteInteraction()
    {
        UIManager.Instance.ShowMessage("Thank you, farewell.");

        isLeaving = true;
        StartCoroutine(DelayedLeave(2f));
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
}