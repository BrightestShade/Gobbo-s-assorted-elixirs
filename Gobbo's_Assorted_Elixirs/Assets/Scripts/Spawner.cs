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

        if (spawnerTimer >= 5f)
        {
            spawnerTimer = 0f;

            if (currentSpawnCount < maxInstances)
            {
                SpawnIngredient();
            }
        }
    }

    public void SpawnIngredient()
    {
        Vector3 spawnPosition = transform.position;
        GameObject ingredient = Instantiate(IngredientPrefab, spawnPosition, Quaternion.identity);

        IngredientItem item = ingredient.GetComponent<IngredientItem>();

        if (item != null)
        {
            item.originSpawner = this;
        }

        currentSpawnCount++;

      //  Debug.Log("Spawned ingredient. Count: " + currentSpawnCount);
    }

    public void DecreaseCount()
    {
        currentSpawnCount--;

        // Prevent negative values
        if (currentSpawnCount < 0)
        {
            currentSpawnCount = 0;
        }

      //  Debug.Log("Ingredient removed. Count: " + currentSpawnCount);
    }
}