using UnityEngine;

public class Spawner : MonoBehaviour
{
     public GameObject IngredientPrefab; 
     public float spawnerTimer;

    private void Update() 
    {
        spawnerTimer += Time.deltaTime;
        if (spawnerTimer > 20)
        {
            SpawnIngredient();
        }

    }

    public void SpawnIngredient()
    {
        spawnerTimer = 0f;
        Debug.Log("spawnerTimer reset");

        Vector3 spawnPosition = transform.position;
        Instantiate(IngredientPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("spawned Ingredient prefab");
    }

}
