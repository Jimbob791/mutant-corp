using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Config")]
    public int maxHealth = 100;
    public int health;
    [SerializeField] float flashTime;

    SpriteRenderer sr;
    bool alive = true;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        alive = health > 0;
        if (!alive)
        {
            Death();
        }

        StartCoroutine(HitFlash());
    }

    private IEnumerator HitFlash()
    {
        sr.material.SetInt("_hit", 1);
        yield return new WaitForSeconds(flashTime);
        sr.material.SetInt("_hit", 0);
    }   

    private void Death()
    {
        Destroy(this.gameObject);
    }
}
