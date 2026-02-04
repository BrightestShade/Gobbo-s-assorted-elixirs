using UnityEngine;

public class MushroomSpawner : MonoBehaviour
{
     public GameObject Red_Mushroom_ClusterPrefab;

    public float spawnerTimer;

    private void Update() 
    {
        spawnerTimer += Time.deltaTime;
        if (spawnerTimer > 20)
        {
            SpawnMushrooms();
        }

    }

    public void SpawnMushrooms()
    {
        spawnerTimer = 0f;
        Debug.Log("spawnerTimer reset");

        Instantiate(Red_Mushroom_ClusterPrefab, transform);
    }

}
