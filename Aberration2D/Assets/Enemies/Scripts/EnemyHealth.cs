using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Config")]
    public int maxHealth = 100;
    public int health;
    public GameObject healthDrop;
    public int data;
    [SerializeField] GameObject dataPrefab;
    [SerializeField] float flashTime;
    public int bleedDamage = 0;
    [SerializeField] GameObject deathFX;
    [SerializeField] GameObject hitFX;
    [SerializeField] GameObject bleedFX;
    [SerializeField] GameObject deathSFX;
    [SerializeField] GameObject damageIndicator;

    SpriteRenderer sr;
    bool alive = true;
    public bool explode = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        health = maxHealth;
        StartCoroutine(Bleed());
    }

    IEnumerator Bleed()
    {
        yield return new WaitForSeconds(1);
        if (bleedDamage != 0)
            TakeDamage(bleedDamage, false);
        StartCoroutine(Bleed());
    }

    public void TakeDamage(int amount, bool flash)
    {
        GameManager.instance.damageDealt += amount;
        health -= amount;

        alive = health > 0;
        if (!alive)
        {
            Death();
            return;
        }

        GameObject num = Instantiate(damageIndicator, transform.position, Quaternion.identity);
        num.GetComponent<DamageNum>().damage = amount;

        if (!flash)
        {
            Instantiate(bleedFX, transform.position, Quaternion.identity);
            return;
        }
        GameManager.instance.TimeFreeze(0.01f);
        Instantiate(hitFX, transform.position, Quaternion.identity);

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
        GameManager.instance.Shake(0.1f, 0.1f);
        GameManager.instance.TimeFreeze(0.1f);
        GameManager.instance.numEnemiesKilled += 1;
        if (explode)
        {
            GameObject newDrop = Instantiate(healthDrop, transform.position, Quaternion.identity);
            newDrop.GetComponent<HealthDrop>().healAmount = Mathf.RoundToInt(maxHealth / 50);
        }
        Instantiate(deathFX, transform.position, Quaternion.identity);
        Instantiate(deathSFX);
        for (int i = 0; i < data; i++)
        {
            Instantiate(dataPrefab, transform.position + new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f), 0), Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
