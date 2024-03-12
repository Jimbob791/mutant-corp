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
        waveTimer = Random.Range(10, 15f);
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
        credits += creditMultiplier * 3 * (1 + 0.4f * difficultyMultiplier);
    }

    private void SpawnWave()
    {
        int sumWeights = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            sumWeights += enemies[i].weight;
        }

        for (int k = 0; k < 50; k++)
        {
            int chosenWeight = Random.Range(0, sumWeights);
            for (int i = 0; i < enemies.Count; i++)
            {
                if (chosenWeight < enemies[i].weight)
                {
                    chosenEnemy = enemies[i];
                    if (chosenEnemy.cost < credits / 4)
                        chosenEnemy = null;
                    else if (chosenEnemy.cost > credits)
                        chosenEnemy = null;
                    break;
                }
                chosenWeight -= enemies[i].weight;
            }

            if (chosenEnemy != null)
            {
                while (credits >= chosenEnemy.cost)
                {
                    int randPosIndex = Random.Range(0, spawnPosList.Count);
                    credits -= chosenEnemy.cost;

                    if (Vector3.Distance(spawnPosList[randPosIndex].position, Player.instance.transform.position) < 15)
                    {
                        SpawnEnemy(chosenEnemy, spawnPosList[randPosIndex]);
                    }
                }
            }
        }
    }

    private void SpawnEnemy(Enemy enemy, Transform chosenTransform)
    {
        GameObject warn = Instantiate(warning, chosenTransform.position + new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f), 0), Quaternion.identity);
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