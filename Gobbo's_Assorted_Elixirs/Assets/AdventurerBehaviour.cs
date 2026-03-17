using UnityEngine;
using System.Collections;

public class AdventurerBehaviour : MonoBehaviour
{
    public Transform targetPoint;
    public float speed = 2f;

    public PotionRecipe requestedPotion;
    public PotionSelector potionSelector;

    private AdventurerSpawner spawner;
    public Transform exitPoint;

    // The point the adventurer looks at while waiting for the potion
    public Transform lookPoint;

    private CauldronIngredientChecker cauldron;
    private bool isLeaving = false;

    void Start()
    {
        spawner = FindObjectOfType<AdventurerSpawner>();
        cauldron = FindObjectOfType<CauldronIngredientChecker>();

        StartCoroutine(MoveAndWait());
    }

    IEnumerator MoveAndWait()
    {
        // Walk to the target point while facing it
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

        // Fully face the target point
        transform.LookAt(targetPoint);

        // Request the potion
        RequestPotion();

        // While waiting for potion, keep looking at lookPoint
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

    void RequestPotion()
    {
        if (requestedPotion == null)
        {
            Debug.LogError("No potion assigned!");
            return;
        }

        potionSelector.SetPotion(requestedPotion);
        Debug.Log("Adventurer requested: " + requestedPotion.name);

        switch (requestedPotion.name)
        {
            case "HealthPotion":
                UIManager.Instance.ShowMessage(
                    "I am going to fight something very dangerous, do you have a potion that might help me stay alive?"
                );
                break;

            case "InvisPotion":
                UIManager.Instance.ShowMessage(
                    "I need to sneak past a dangerous creature, can you help me out?"
                );
                break;
        }

        cauldron.OnPotionComplete += OnPotionFinished;
    }

    void OnPotionFinished()
    {
        if (isLeaving) return;

        isLeaving = true;
        Debug.Log("Potion received! Leaving...");
        StartCoroutine(Leave());
    }

    IEnumerator Leave()
    {
        cauldron.OnPotionComplete -= OnPotionFinished;

        while (Vector3.Distance(transform.position, exitPoint.position) > 0.1f)
        {
            // Rotate toward the exit point while walking
            Vector3 direction = (exitPoint.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // Move toward exit
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
}