using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<Enemy> enemies = new List<Enemy>();

    [SerializeField] List<Transform> spawnPosList = new List<Transform>();

    void Update()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].timeSinceSpawned += Time.deltaTime;
            
            if (enemies[i].timeSinceSpawned > enemies[i].chosenSpawnTime)
            {
                SpawnEnemy(enemies[i].prefab);
                enemies[i].timeSinceSpawned = 0;
                enemies[i].chosenSpawnTime = Random.Range(enemies[i].minSpawnTime, enemies[i].maxSpawnTime);
            }
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        Vector3 spawnPos = spawnPosList[Random.Range(0, spawnPosList.Count)].position;

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject prefab;
    public float minSpawnTime;
    public float maxSpawnTime;
    public float chosenSpawnTime;
    public float timeSinceSpawned;
}