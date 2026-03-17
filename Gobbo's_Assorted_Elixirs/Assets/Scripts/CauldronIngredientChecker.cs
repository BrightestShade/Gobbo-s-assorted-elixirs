using UnityEngine;
using System.Collections.Generic;

public class CauldronIngredientChecker : MonoBehaviour
{
    [Header("Active Recipe")]
    public PotionRecipe currentRecipe;

    private List<IngredientData> addedIngredients = new List<IngredientData>();
    private bool potionComplete = false;
    [SerializeField] private Spawner spawner;

    public System.Action OnPotionComplete;


    private void OnTriggerEnter(Collider other)
    {
        IngredientItem item = other.GetComponent<IngredientItem>();
        if (item == null) return;

        // If no recipe potion finished reject ingredient
        if (currentRecipe == null || potionComplete)
        {
            SpitOutIngredient(other.gameObject);
            return;
        }

        // Disable collider to stop multiple triggers
        Collider col = other.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        CheckIngredient(item.ingredientData);

        if (item.originSpawner != null)
        {
            item.originSpawner.DecreaseCount();
        }

        Destroy(other.gameObject);

        Debug.Log("Ingredient destroyed");
    }

    void CheckIngredient(IngredientData ingredient)
    {
        if (currentRecipe == null) return;

        int step = addedIngredients.Count;

        // Check if we are beyond recipe length
        if (step >= currentRecipe.requiredIngredients.Count)
        {
            FailPotion();
            return;
        }

        IngredientData requiredIngredient = currentRecipe.requiredIngredients[step];

        // Compare by name instead of reference
        if (ingredient.ingredientName == requiredIngredient.ingredientName)
        {
            addedIngredients.Add(ingredient);
            GoodIngredientFeedback();

            if (addedIngredients.Count == currentRecipe.requiredIngredients.Count)
            {
                CompletePotion();
                
            }
        }
        else
        {
            FailPotion();
            
        }
    }

    void GoodIngredientFeedback()
    {
        Debug.Log("Correct ingredient");
        // Flash blue
        // Play bubbly sound
    }

    void FailPotion()
    {
        Debug.Log("Wrong ingredient and potion ruined.");
        addedIngredients.Clear();
    }

    void CompletePotion()
    {
        Debug.Log("Potion completed");
        addedIngredients.Clear();
        potionComplete = true;

        OnPotionComplete?.Invoke(); 
    }


    public void ResetPotion()
    {
        addedIngredients.Clear();
        potionComplete = false;
    }


    void SpitOutIngredient(GameObject ingredient)
    {
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 spitDirection = (ingredient.transform.position - transform.position).normalized;
            spitDirection += Vector3.up * 0.5f;

            rb.linearVelocity = Vector3.zero;
            rb.AddForce(spitDirection * 2f, ForceMode.Impulse);
        }

        Debug.Log("Cauldron rejected ingredient");
    }

    public bool IsPotionComplete()
    {
        return potionComplete;
    }
    // For future NPC system
    /* public void SetActiveRecipe(PotionRecipe newRecipe)
     {
         currentRecipe = newRecipe;
         addedIngredients.Clear();
         potionComplete = false;
     } */
}