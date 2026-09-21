using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;   // drag both Enemy and FlyingEnemy prefabs in
    public Transform[] spawnPoints;
    public float spawnInterval = 4f;
    public int maxAliveEnemies = 6;

    private List<GameObject> aliveEnemies = new List<GameObject>();
    private float nextSpawnTime;

    void Update()
    {
        aliveEnemies.RemoveAll(e => e == null); // clean up destroyed enemies from the list

        if (Time.time >= nextSpawnTime && aliveEnemies.Count < maxAliveEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject newEnemy = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
        aliveEnemies.Add(newEnemy);
    }
}