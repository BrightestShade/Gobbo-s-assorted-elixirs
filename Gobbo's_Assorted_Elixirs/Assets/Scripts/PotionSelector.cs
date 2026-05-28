using UnityEngine;

public class PotionSelector : MonoBehaviour
{
    public static PotionSelector Instance;

    public CauldronIngredientChecker cauldron;

    public PotionRecipe currentSelectedPotion;

    private void Awake()
    {
        Instance = this;
    }

    public void SetPotion(PotionRecipe recipe)
    {
        if (recipe != null)
        {
            currentSelectedPotion = recipe;

            cauldron.currentRecipe = recipe;
            cauldron.ResetPotion();

            Debug.Log("Selected Recipe: " + recipe.potionName);

            UIManager.Instance.ShowMessage(recipe.potionName + " selected.");
        }
    }
}