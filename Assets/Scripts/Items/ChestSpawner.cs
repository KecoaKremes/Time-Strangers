using UnityEngine;
using System.Collections.Generic;

public class ChestSpawner : MonoBehaviour
{
    public GameObject chestPrefab;
    public Transform[] possibleSpawnPoints;
    public int chestsToSpawn = 4;

    void Start()
    {
        List<Transform> shuffled = new List<Transform>(possibleSpawnPoints);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[randomIndex]) = (shuffled[randomIndex], shuffled[i]);
        }

        int count = Mathf.Min(chestsToSpawn, shuffled.Count);
        for (int i = 0; i < count; i++)
        {
            Instantiate(chestPrefab, shuffled[i].position, Quaternion.identity);
        }
    }
}