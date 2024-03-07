using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float spawnDistance;
    [SerializeField] List<Enemy> enemies = new List<Enemy>();

    [SerializeField] List<Transform> spawnPosList = new List<Transform>();

    int iterations = 50;
    Transform chosenTransform;

    void Start()
    {
        foreach (Transform child in transform)
        {
            spawnPosList.Add(child);
        }
    }

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
        for (int i = 0; i < iterations; i++)
        {
            int index = Random.Range(0, spawnPosList.Count);
            if (Vector3.Distance(spawnPosList[index].position, Player.instance.transform.position) < spawnDistance)
            {
                chosenTransform = spawnPosList[index];
                break;
            }
        }

        Instantiate(prefab, chosenTransform.position, Quaternion.identity);
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