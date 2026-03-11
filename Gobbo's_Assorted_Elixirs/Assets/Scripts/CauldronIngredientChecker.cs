using UnityEngine;
using System.Collections.Generic;

public class CauldronIngredientChecker : MonoBehaviour
{
    [Header("Active Recipe")]
    public PotionRecipe currentRecipe;

    private List<IngredientData> addedIngredients = new List<IngredientData>();
    private bool potionComplete = false;
    [SerializeField] private Spawner spawner;
    private void OnTriggerEnter(Collider other)
    {
        if (potionComplete) return;

        IngredientItem item = other.GetComponent<IngredientItem>();
        if (item == null) return;

        CheckIngredient(item.ingredientData);

        if (item.originSpawner != null)
        {
            item.originSpawner.DecreaseCount();
        }

        Destroy(other.gameObject);

        Debug.Log("Ingredient used");
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

    if (ingredient == requiredIngredient)
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
    }

    // For future NPC system
   /* public void SetActiveRecipe(PotionRecipe newRecipe)
    {
        currentRecipe = newRecipe;
        addedIngredients.Clear();
        potionComplete = false;
    } */
}