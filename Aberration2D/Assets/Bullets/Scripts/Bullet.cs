using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Vector3 desiredVelocity;
    public int damage;
    public float speed;
    public float homingStrength;
    [SerializeField] float gravity;
    [SerializeField] float acceleration;
    [SerializeField] GameObject damageIndicator;
    [SerializeField] GameObject deathFX;
    [SerializeField] GameObject hitSFX;

    GameObject[] enemies;
    Rigidbody2D rb;
    Transform closestEnemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 10f);
    }

    void Update()
    {
        float rotZ = Mathf.Atan2(desiredVelocity.y, desiredVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        rb.velocity = desiredVelocity * speed;
    }

    void FixedUpdate()
    {
        speed += acceleration;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        closestEnemy = GetClosestEnemyTransform(enemies);
        if (homingStrength != 0 && closestEnemy != null)
        {
            Vector2 diff = closestEnemy.position - transform.position;
            diff.Normalize();
            desiredVelocity = DataFragment.RotateTowards(desiredVelocity, diff, homingStrength * Mathf.Deg2Rad, 0);
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Platforms")
        {
            if (GetComponent<Grenade>() != null)
            {
                GetComponent<Grenade>().damage = damage;
                GetComponent<Grenade>().Explode();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (GetComponent<Grenade>() != null)
            {
                GetComponent<Grenade>().damage = damage;
                Player.instance.GetComponent<PlayerItems>().Hit(col.gameObject);
                GetComponent<Grenade>().Explode();
            }
            else
            {
                Instantiate(hitSFX);
                col.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage, true);
                Player.instance.GetComponent<PlayerHealth>().TakeDamage(Player.instance.GetComponent<PlayerHealth>().lifeSteal * -1, true);
                Player.instance.GetComponent<PlayerItems>().Hit(col.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    void OnDisable()
    {
        if(!this.gameObject.scene.isLoaded) return;
        if (deathFX != null) Instantiate(deathFX, transform.position, Quaternion.identity);
    }
}
