using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] float hoverDistance;
    [SerializeField] float rotationPeriod;
    [SerializeField] float aggroDistance;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject explosionSFX;
    [SerializeField] GameObject damageIndicator;
    [SerializeField] LayerMask enemyLayer;
    public int damage;
    public int index;
    public int numDrones;

    Vector3 startVector = new Vector3(1, 0, 0);
    public float offset = 0;
    bool rotate = true;
    float speed = 0;
    Transform closest;

    void FixedUpdate()
    {
        if (rotate)
        {
            offset += Time.fixedDeltaTime;
            if (offset > rotationPeriod)
            {
                offset = 0;
            }

            float degrees = -1 * index * (360 / numDrones) + ((offset / rotationPeriod) * 360);

            Vector3 newRot = Quaternion.AngleAxis(degrees, new Vector3(0, 0, 1)) * startVector;
            transform.position = Vector3.Lerp(transform.position, Player.instance.transform.position + newRot * hoverDistance, 0.05f);
            closest = GetClosestEnemyTransform(GameObject.FindGameObjectsWithTag("Enemy"));
            if (closest == null)
            {
                rotate = true;
                return;
            }
            if (Vector3.Distance(closest.position, transform.position) < aggroDistance)
            {
                rotate = false;
            }
        }
        else
        {
            closest = GetClosestEnemyTransform(GameObject.FindGameObjectsWithTag("Enemy"));
            if (closest == null)
            {
                rotate = true;
                return;
            }
            Vector3 diff = closest.position - transform.position;
            diff.Normalize();

            transform.position = transform.position + (diff * speed);
            speed += 0.05f;

            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.2f, Vector2.zero, 0, enemyLayer);
            if (hits.Length != 0)
            {
                GameObject effect = Instantiate(explosion, transform.position, Quaternion.identity);
                for (int i = 0; i < hits.Length; i++)
                {
                    hits[i].transform.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage, true);
                }
                Destroy(effect, 2f);
                Player.instance.GetComponent<PlayerShoot>().drones.Remove(this.gameObject);
                Player.instance.GetComponent<PlayerShoot>().numSpawned -= 1;
                GameObject num = Instantiate(damageIndicator, transform.position, Quaternion.identity);
                num.GetComponent<DamageNum>().damage = damage;
                Instantiate(explosionSFX);
                GameManager.instance.Shake(0.05f, 0.08f);
                Destroy(this.gameObject);
            }
        }
    }

    Transform GetClosestEnemyTransform(GameObject[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject g in enemies)
        {
            float dist = Vector3.Distance(g.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = g.transform;
                minDist = dist;
            }
        }
        return tMin;
    }
}
