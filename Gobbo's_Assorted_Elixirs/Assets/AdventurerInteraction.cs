using UnityEngine;
using TMPro;

public class AdventurerInteraction : MonoBehaviour
{
    private AdventurerBehaviour currentAdventurer;
    private bool playerInside = false;

    public GameObject interactPrompt;
    private TMP_Text promptText;

    void Start()
    {
        if (interactPrompt != null)
            promptText = interactPrompt.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (playerInside && currentAdventurer != null)
        {
            UpdatePromptText();

            if (Input.GetKeyDown(KeyCode.E))
            {
                currentAdventurer.Interact();
            }
        }
    }

    void UpdatePromptText()
    {
        if (currentAdventurer == null || promptText == null) return;

        if (!currentAdventurer.HasGivenOrder())
        {
            promptText.text = "Press E to talk";
        }
        else if (currentAdventurer.IsPotionReady())
        {
            promptText.text = "Press E to deliver potion";
        }
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (interactPrompt != null)
            {
                interactPrompt.SetActive(true);
                UpdatePromptText();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }

    public void SetAdventurer(AdventurerBehaviour adventurer)
    {
        currentAdventurer = adventurer;
        gameObject.SetActive(true);
    }
}