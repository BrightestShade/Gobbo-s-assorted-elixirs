using UnityEngine;

public class PotionSelector : MonoBehaviour
{
    public CauldronIngredientChecker cauldron;

    
    public void SetPotion(PotionRecipe recipe)
    {
        if (recipe != null)
        {
            cauldron.currentRecipe = recipe;
            cauldron.ResetPotion(); 
        }
    }
}