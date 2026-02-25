using UnityEngine;

public class IngredientCollect : MonoBehaviour
{
    public float pickupRange = 2f;
    private GameObject objectInRange;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && objectInRange != null)
        {
            CollectObject();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            objectInRange = other.gameObject;
            Debug.Log("Ingredient in interaction range");
          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            objectInRange = null;
            Debug.Log("Ingredient out of interaction range");
        }
    }

    void CollectObject()
    {
      
        Destroy(objectInRange);
     
        objectInRange = null;

        Debug.Log("Ingredient collected");
    }
}

