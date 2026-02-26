using UnityEngine;
using System.Collections.Generic;

public class CauldronIngredientChecker : MonoBehaviour
{
    [Header("Active Recipe")]
    public PotionRecipe currentRecipe;

    private List<IngredientData> addedIngredients = new List<IngredientData>();
    private bool potionComplete = false;

    private void OnTriggerEnter(Collider other)
    {
        if (potionComplete) return;

        IngredientItem item = other.GetComponent<IngredientItem>();
        if (item == null) return;

        CheckIngredient(item.ingredientData);

       
       // Destroy(other.gameObject);
       // Call the "DecreaseCount()" method from Spawner script
    }

    void CheckIngredient(IngredientData ingredient)
    {
        if (currentRecipe == null) return;

        int requiredCount = currentRecipe.requiredIngredients
            .FindAll(i => i == ingredient).Count;

        int currentCount = addedIngredients
            .FindAll(i => i == ingredient).Count;

        if (currentCount < requiredCount)
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