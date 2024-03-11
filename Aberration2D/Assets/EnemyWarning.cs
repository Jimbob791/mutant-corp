using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWarning : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public float enemyDamageMulti;
    public float enemyHealthMulti;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        GameObject enemy = Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        enemy.GetComponent<EnemyHealth>().maxHealth = Mathf.RoundToInt(enemy.GetComponent<EnemyHealth>().maxHealth * enemyHealthMulti);
        if (enemy.GetComponent<EnemyMelee>() != null)
        {
            enemy.GetComponent<EnemyMelee>().damage = Mathf.RoundToInt(enemy.GetComponent<EnemyMelee>().damage * enemyDamageMulti);
        }
        if (enemy.GetComponent<EnemyRanged>() != null)
        {
            enemy.GetComponent<EnemyRanged>().damage = Mathf.RoundToInt(enemy.GetComponent<EnemyRanged>().damage * enemyDamageMulti);
        }
        ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission;
        em.enabled = false;
        Destroy(this.gameObject, 2f);
    }
}
