using UnityEngine;
using TMPro;

public class RecipeBookInteraction : MonoBehaviour
{
    public GameObject recipeBookUI;
    public GameObject interactPrompt;

    public MonoBehaviour FirstPersonController;

    private bool playerInside = false;

    void Update()
    {
        if (playerInside)
        {
            if (interactPrompt != null)
                interactPrompt.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleRecipeBook();
            }
        }
        else
        {
            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }

    void ToggleRecipeBook()
    {
        bool isOpen = recipeBookUI.activeSelf;

        recipeBookUI.SetActive(!isOpen);

        bool openingBook = !isOpen;


        Cursor.lockState = openingBook ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = openingBook;


        if (FirstPersonController != null)
        {
            FirstPersonController.enabled = !openingBook;
        }


        Time.timeScale = openingBook ? 0f : 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (recipeBookUI.activeSelf)
            {
                recipeBookUI.SetActive(false);

                if (FirstPersonController != null)
                {
                    FirstPersonController.enabled = true;
                }

                Time.timeScale = 1f;
            }
        }
    }
}