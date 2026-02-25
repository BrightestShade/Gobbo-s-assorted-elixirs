using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Potion Recipe")]
public class PotionRecipe : ScriptableObject
{
    public string potionName;
    public List<IngredientData> requiredIngredients;
}