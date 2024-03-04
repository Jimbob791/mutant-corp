using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Config")]
    public int maxHealth = 200;
    public int health;
    public int lifeSteal;
    [SerializeField] float invincibilityTime;

    [Header("Health Display")]
    [SerializeField] HealthBar healthSlider;
    [SerializeField] TextMeshProUGUI currentText;
    [SerializeField] TextMeshProUGUI maxText;

    SpriteRenderer sr;

    bool alive = true;
    bool invincible = false;

    void Start()
    {
        health = maxHealth;
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        SetHealth();
    }

    public void SetHealth()
    {
        healthSlider.SetValues(maxHealth, health);

        maxText.text = maxHealth.ToString();
        currentText.text = health.ToString();
    }

    public void TakeDamage(int amount, bool ignoreImmunity)
    {
        if (!ignoreImmunity)
        {
            if (invincible || GetComponent<PlayerRoll>().rolling)
            {
                return;
            }
        }

        health -= amount;

        alive = health > 0;
        if (!alive)
        {
            health = 0;
            Death();
        }

        SetHealth();
        if (!ignoreImmunity)
        {
            healthSlider.ShakeBar();
            StartCoroutine(IFrames());
        }
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
