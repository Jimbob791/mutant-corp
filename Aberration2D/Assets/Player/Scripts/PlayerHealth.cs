using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Config")]
    public int maxHealth = 200;
    public int health;
    [SerializeField] float invincibilityTime;

    SpriteRenderer sr;

    bool alive = true;
    bool invincible = false;

    void Start()
    {
        health = maxHealth;
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        if (invincible || GetComponent<PlayerRoll>().rolling)
        {
            return;
        }

        health -= amount;

        alive = health > 0;
        if (!alive)
        {
            Death();
        }

        StartCoroutine(IFrames());
    }

    private IEnumerator IFrames()
    {
        invincible = true;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(invincibilityTime / 10);
            sr.enabled = !sr.enabled;
        }
        invincible = false;
    }   

    private void Death()
    {
        Time.timeScale = 0.2f;
        Debug.Log("Death");
    }
}
