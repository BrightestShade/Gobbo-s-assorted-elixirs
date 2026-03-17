using UnityEngine;

public class RecipeBookInteractable : MonoBehaviour
{
    public GameObject recipeBookUI; 
    public bool isOpen = false;

    private void OnMouseDown()
    {
        ToggleBook();
    }

    void ToggleBook()
    {
        isOpen = !isOpen;
        recipeBookUI.SetActive(isOpen);

        
        PlayerController.Instance.SetInputEnabled(!isOpen); // lock movement
    }
}