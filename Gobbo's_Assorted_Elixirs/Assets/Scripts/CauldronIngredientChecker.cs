using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CauldronIngredientChecker : MonoBehaviour
{
    [Header("Active Recipe")]
    public PotionRecipe currentRecipe;

    private List<IngredientData> addedIngredients = new List<IngredientData>();
    private bool potionComplete = false;
    [SerializeField] private Spawner spawner;

    public System.Action OnPotionComplete;

    [SerializeField] private ParticleSystem smokeParticles;

    private ParticleSystem.MinMaxGradient defaultSmokeColor;

    public TMP_Text successText;

    public PotionRecipe brewedPotion;

    void Start()
    {
        if (smokeParticles != null)
        {
            var main = smokeParticles.main;
            defaultSmokeColor = main.startColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IngredientItem item = other.GetComponent<IngredientItem>();
        if (item == null) return;

        
        if (currentRecipe == null || potionComplete)
        {
            SpitOutIngredient(other.gameObject);
            return;
        }

        
        Collider col = other.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        CheckIngredient(item.ingredientData);

        if (item.originSpawner != null)
        {
            item.originSpawner.DecreaseCount();
        }

        Destroy(other.gameObject);

        Debug.Log("Ingredient destroyed");
    }

    void CheckIngredient(IngredientData ingredient)
    {
        if (currentRecipe == null) return;

        int step = addedIngredients.Count;



       
        if (step >= currentRecipe.requiredIngredients.Count)
        {
            FailPotion();
            return;
        }

        IngredientData requiredIngredient = currentRecipe.requiredIngredients[step];


        if (ingredient == requiredIngredient)
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
        Debug.Log("Correct ingredient");

        StartCoroutine(FlashSmokeColor(Color.green,2f));

        // add bubbly sound later
    }

    void FailPotion()
    {
        Debug.Log("Wrong ingredient and potion ruined.");

        addedIngredients.Clear();

        StartCoroutine(FlashSmokeColor(Color.red, 2f));
    }

    void CompletePotion()
    {
        Debug.Log("Potion completed");

        brewedPotion = currentRecipe;

        addedIngredients.Clear();
        potionComplete = true;

        OnPotionComplete?.Invoke();
    }


    public void ResetPotion()
    {
        addedIngredients.Clear();
        potionComplete = false;
    }


    void SpitOutIngredient(GameObject ingredient)
    {
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 spitDirection = (ingredient.transform.position - transform.position).normalized;
            spitDirection += Vector3.up * 0.5f;

            rb.linearVelocity = Vector3.zero;
            rb.AddForce(spitDirection * 2f, ForceMode.Impulse);
        }

        Debug.Log("Cauldron rejected ingredient");
    }

    public bool IsPotionComplete()
    {
        return potionComplete;
    }

    IEnumerator FlashSmokeColor(Color flashColor, float duration)
    {
        if (smokeParticles == null) yield break;

        var main = smokeParticles.main;

        
        main.startColor = flashColor;

        yield return new WaitForSeconds(duration);

       
        main.startColor = defaultSmokeColor;
    }

}