using UnityEngine;

public class RecipePageButton : MonoBehaviour
{
    public PotionRecipe recipe;

    public GameObject recipeBookUI;

    public MonoBehaviour firstPersonController;

    public void SelectRecipe()
    {
        PotionSelector.Instance.SetPotion(recipe);

        recipeBookUI.SetActive(false);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        if (firstPersonController != null)
        {
            firstPersonController.enabled = true;
        }


        Time.timeScale = 1f;
    }
}