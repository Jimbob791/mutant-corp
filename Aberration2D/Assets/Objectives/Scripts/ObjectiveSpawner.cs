using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSpawner : MonoBehaviour
{
    float stage;
    public GameObject warning;
    public float difficultyMultiplier;

    [Space]

    [SerializeField] List<Enemy> enemies = new List<Enemy>();
    [SerializeField] List<Transform> spawnPosList = new List<Transform>();

    // int iterations = 50;
    Transform chosenTransform;
    [SerializeField] float credits;
    Enemy chosenEnemy;

    void Start()
    {
        stage = Mathf.Log(difficultyMultiplier, 1.5f) + 2;
        credits = stage * 100;
        foreach (Transform child in transform)
        {
            if (child.gameObject.name != "DrillCanvas" && child.gameObject.name != "Effect")
                spawnPosList.Add(child);
        }
    }

    public void SpawnWave()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        for (int i = 0; i < spawners.Length; i++)
        {
            if (spawners[i].GetComponent<EnemySpawner>() != null)
            {
                EnemySpawner spawner = spawners[i].GetComponent<EnemySpawner>();
                spawner.waveInterval = 8;
                spawner.creditMultiplier = spawner.creditMultiplier * 1.2f;
            }
        }

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
                    break;
                }
                chosenWeight -= enemies[i].weight;
            }

            if (chosenEnemy != null)
            {
                while (credits >= chosenEnemy.cost)
                {
                    int randPosIndex = Random.Range(0, spawnPosList.Count);

                    if (Vector3.Distance(spawnPosList[randPosIndex].position, Player.instance.transform.position) < 15)
                    {
                        SpawnEnemy(chosenEnemy, spawnPosList[randPosIndex]);
                        credits -= chosenEnemy.cost;
                    }
                }
            }
        }
    }

    private void SpawnEnemy(Enemy enemy, Transform chosenTransform)
    {
        GameObject warn = Instantiate(warning, chosenTransform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f), 0), Quaternion.identity);
        warn.GetComponent<EnemyWarning>().enemyToSpawn = enemy.prefab;
        warn.GetComponent<EnemyWarning>().enemyHealthMulti = difficultyMultiplier + 0.4f;
        warn.GetComponent<EnemyWarning>().enemyDamageMulti = 0.5f * difficultyMultiplier  + 0.6f;
    }
}
