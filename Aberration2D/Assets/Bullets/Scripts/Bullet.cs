using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public Vector3 desiredVelocity;
    public int damage;
    public float speed;
    public float homingStrength;

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
        rb.velocity = desiredVelocity * speed;
    }

    void FixedUpdate()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        closestEnemy = GetClosestEnemyTransform(enemies);
        if (homingStrength != 0 && closestEnemy != null)
        {
            desiredVelocity = Vector3.Lerp(desiredVelocity, closestEnemy.transform.position - transform.position, homingStrength / 10);
            desiredVelocity.Normalize();
            float rotZ = Mathf.Atan2(desiredVelocity.y, desiredVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
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
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            Player.instance.GetComponent<PlayerHealth>().TakeDamage(Player.instance.GetComponent<PlayerHealth>().lifeSteal * -1, true);
            Destroy(this.gameObject);
        }
    }
}
