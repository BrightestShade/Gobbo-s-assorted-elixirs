using UnityEngine;
using System.Collections.Generic;

public class CauldronIngredientChecker : MonoBehaviour
{
    [Header("Current Recipe")]
    public PotionRecipe currentRecipe;

    private List<IngredientData> addedIngredients = new List<IngredientData>();

    private void OnTriggerEnter(Collider other)
    {
        IngredientItem item = other.GetComponent<IngredientItem>();
        if (item == null) return;

        CheckIngredient(item.ingredientData);
        Destroy(other.gameObject);
    }

    void CheckIngredient(IngredientData ingredient)
    {
        if (currentRecipe == null) return;

        int requiredCount = currentRecipe.requiredIngredients.FindAll(i => i == ingredient).Count;
        int currentCount = addedIngredients.FindAll(i => i == ingredient).Count;

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
        Debug.Log("Correct Ingredient!");

        // Flash blue
        // Play bubbly sound
    }

    void FailPotion()
    {
        Debug.Log("Wrong Ingredient! Potion ruined.");
        addedIngredients.Clear();
        // Play belch sound
    }

    void CompletePotion()
    {
        Debug.Log("Potion Completed!");
        addedIngredients.Clear();
    }
}