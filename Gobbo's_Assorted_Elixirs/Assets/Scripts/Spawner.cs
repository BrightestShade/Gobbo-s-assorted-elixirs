using UnityEngine;

public class Spawner : MonoBehaviour
{
     public GameObject IngredientPrefab; 
     public float spawnerTimer;
     public int maxInstances;


    private int currentSpawnCount = 0;
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
       
        if (currentSpawnCount >= maxInstances)
        {
            return;
        }

        Vector3 spawnPosition = transform.position;
        Instantiate(IngredientPrefab, spawnPosition, Quaternion.identity);
        currentSpawnCount++;
        Debug.Log("spawned Ingredient prefab");
    }

    public void DecreaseCount()
    {
        currentSpawnCount--;
    }
}
