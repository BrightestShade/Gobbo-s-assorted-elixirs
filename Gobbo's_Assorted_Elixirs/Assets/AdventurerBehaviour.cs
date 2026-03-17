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
        // Walk to window
        while (Vector3.Distance(transform.position, targetPoint.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPoint.position,
                speed * Time.deltaTime
            );

            yield return null;
        }

        
        RequestPotion();

        
        cauldron.OnPotionComplete += OnPotionFinished;
    }

    void RequestPotion()
    {
        if (requestedPotion == null)
        {
            Debug.LogError("No potion assigned!");
            return;
        }

        potionSelector.SetPotion(requestedPotion);

        Debug.Log("Adventurer requested: " + requestedPotion.potionName);
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