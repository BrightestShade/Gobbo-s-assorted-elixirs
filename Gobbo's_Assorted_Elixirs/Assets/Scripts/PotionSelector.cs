using UnityEngine;

public class PotionSelector : MonoBehaviour
{
    public CauldronIngredientChecker cauldron;

    public void SetPotion(PotionRecipe recipe)
    {
        cauldron.currentRecipe = recipe;
    }
}