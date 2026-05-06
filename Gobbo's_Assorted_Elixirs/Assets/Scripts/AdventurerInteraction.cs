using UnityEngine;
using TMPro;

public class AdventurerInteraction : MonoBehaviour
{
    private AdventurerBehaviour currentAdventurer;
    private bool playerInside = false;

    public GameObject interactPrompt;
    private TMP_Text promptText;

    private bool interactionLocked = false;
    private bool interactionComplete = false;

    void Start()
    {
        if (interactPrompt != null)
            promptText = interactPrompt.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (playerInside && currentAdventurer != null && !interactionComplete)
        {
            if (!currentAdventurer.HasGivenOrder() || currentAdventurer.IsPotionReady())
            {
                if (interactPrompt != null && !interactPrompt.activeSelf)
                    interactPrompt.SetActive(true);

                UpdatePromptText();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentAdventurer.Interact();

                    if (interactPrompt != null)
                        interactPrompt.SetActive(false);

                    
                    if (currentAdventurer.IsPotionReady())
                    {
                        interactionComplete = true;
                    }
                }
            }
            else
            {
                if (interactPrompt != null)
                    interactPrompt.SetActive(false);
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

          
            interactionLocked = false;

            if (interactPrompt != null && !interactPrompt.activeSelf)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (interactPrompt != null)
            {
                UpdatePromptText();

                if ((!currentAdventurer.HasGivenOrder() || currentAdventurer.IsPotionReady()) && !interactionComplete)
                {
                    if (interactPrompt != null)
                        interactPrompt.SetActive(true);
                }
                else
                {
                    if (interactPrompt != null)
                        interactPrompt.SetActive(false);
                }
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
        interactionComplete = false; // resets the interaction complete for new NPC allowing for the player to interact with them again
        gameObject.SetActive(true);
    }
}