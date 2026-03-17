using UnityEngine;
using System.Collections;

public class AdventurerSpawner : MonoBehaviour
{
    public GameObject adventurerPrefab;
    public Transform spawnPoint;
    public Transform targetPoint;
    private GameObject currentAdventurer;

    public PotionSelector potionSelector;
    public PotionRecipe[] possiblePotions;

    public Transform exitPoint;
    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (currentAdventurer == null)
            {
                SpawnAdventurer();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnAdventurer()
    {
        currentAdventurer = Instantiate(
            adventurerPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        AdventurerBehaviour script = currentAdventurer.GetComponent<AdventurerBehaviour>();
        script.targetPoint = targetPoint;
        script.requestedPotion = GetRandomPotion();
        script.potionSelector = potionSelector;
        script.exitPoint = exitPoint;
    }

    public void AdventurerFinished()
    {
        currentAdventurer = null;
    }

    PotionRecipe GetRandomPotion()
    {
        return possiblePotions[Random.Range(0, possiblePotions.Length)];
    }

}