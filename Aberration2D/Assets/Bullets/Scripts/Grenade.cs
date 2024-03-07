using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public int damage;
    [SerializeField] GameObject explosion;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float explosionRange;

    public void Explode()
    {
        GameObject effect = Instantiate(explosion, transform.position, Quaternion.identity);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRange, Vector2.zero, 0, enemyLayer);
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].transform.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
        Destroy(effect, 2f);
        Destroy(this.gameObject);
    }
}
