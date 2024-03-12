using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float spawnDistance;
    [SerializeField] float waveInterval;
    [SerializeField] float creditMultiplier;
    public float difficultyMultiplier;
    public GameObject warning;

    [Space]

    [SerializeField] List<Enemy> enemies = new List<Enemy>();
    [SerializeField] List<Transform> spawnPosList = new List<Transform>();

    // int iterations = 50;
    Transform chosenTransform;
    float creditTimer = 0;
    float waveTimer = 999;
    [SerializeField] float credits;
    Enemy chosenEnemy;

    void Start()
    {
        credits = 30;
        foreach (Transform child in transform)
        {
            spawnPosList.Add(child);
        }
    }

    void Update()
    {
        creditTimer += Time.deltaTime;
        waveTimer += Time.deltaTime;

        if (creditTimer > 1)
        {
            AddCredits();
            creditTimer = 0;
        }

        if (waveTimer > waveInterval)
        {
            SpawnWave();
            waveTimer = 0;
        }
    }

    private void AddCredits()
    {
        credits += creditMultiplier * (1 + 0.4f * difficultyMultiplier);
    }

    private void SpawnWave()
    {
        int sumWeights = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            sumWeights += enemies[i].weight;
        }

        while (credits > 0)
        {
            int iterations = 0;
            int chosenWeight = Random.Range(0, sumWeights);
            Debug.Log(chosenWeight);
            for (int i = 0; i < enemies.Count; i++)
            {
                if (chosenWeight < enemies[i].weight && credits >= enemies[i].cost && credits < enemies[i].cost * 4)
                {
                    chosenEnemy = enemies[i];
                    break;
                }
                
                chosenWeight -= enemies[i].weight;
            }

            iterations += 1;
            if (iterations > 50)
            {
                return;
            }

            if (chosenEnemy != null)
            {
                credits -= chosenEnemy.cost;

                while (true)
                {
                    int randPosIndex = Random.Range(0, spawnPosList.Count);
                    if (Vector3.Distance(spawnPosList[randPosIndex].position, Player.instance.transform.position) < 20)
                    {
                        SpawnEnemy(chosenEnemy, spawnPosList[randPosIndex]);
                        break;
                    }
                }
            }
        }
    }

    private void SpawnEnemy(Enemy enemy, Transform chosenTransform)
    {
        GameObject warn = Instantiate(warning, chosenTransform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0), Quaternion.identity);
        warn.GetComponent<EnemyWarning>().enemyToSpawn = enemy.prefab;
        warn.GetComponent<EnemyWarning>().enemyHealthMulti = 0.8f * difficultyMultiplier + 0.5f;
        warn.GetComponent<EnemyWarning>().enemyDamageMulti = 0.3f * difficultyMultiplier  + 0.7f;
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject prefab;
    public int cost;
    public int weight;
}