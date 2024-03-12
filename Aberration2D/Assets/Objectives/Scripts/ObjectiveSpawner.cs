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
        int sumWeights = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            sumWeights += enemies[i].weight;
        }

        while (credits > 0)
        {
            int iterations = 0;
            int chosenWeight = Random.Range(0, sumWeights);
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
        warn.GetComponent<EnemyWarning>().enemyHealthMulti = 0.8f * difficultyMultiplier + 0.6f;
        warn.GetComponent<EnemyWarning>().enemyDamageMulti = 0.3f * difficultyMultiplier  + 0.8f;
    }
}
