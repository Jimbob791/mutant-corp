using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Config")]
    public int maxHealth = 200;
    public int health;
    public int regen = 1;
    public int lifeSteal;
    public int lives;
    [SerializeField] float invincibilityTime;

    [Header("Health Display")]
    [SerializeField] HealthBar healthSlider;
    [SerializeField] TextMeshProUGUI currentText;
    [SerializeField] TextMeshProUGUI maxText;
    [SerializeField] GameObject hurtSFX;
    private Vignette vig;
    public Volume globalPostProcess;

    SpriteRenderer sr;

    bool alive = true;
    bool invincible = false;

    void Start()
    {
        globalPostProcess.profile.TryGet(out vig);
        health = maxHealth;
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        SetHealth();
        StartCoroutine(Regen());
    }

    public void SetHealth()
    {
        healthSlider.SetValues(maxHealth, health);

        maxText.text = maxHealth.ToString();
        currentText.text = health.ToString();
    }

    public void TakeDamage(int amount, bool ignoreImmunity)
    {
        if (amount >= maxHealth && health > maxHealth * 0.9f)
        {
            amount = Mathf.RoundToInt(maxHealth * 0.9f);
        }
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
            if (lives == 0)
            {
                health = 0;
                Death();
            }
            else
            {
                health = maxHealth;
                lives -= 1;
            }
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        SetHealth();
        if (!ignoreImmunity)
        {
            Instantiate(hurtSFX);
            StartCoroutine(HitVignette());
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
        Time.timeScale = 0.8f;
        Debug.Log("Death");
        GameManager.instance.LoadMenu();
    }

    IEnumerator Regen()
    {
        yield return new WaitForSeconds(5);
        TakeDamage(-regen, true);
        StartCoroutine(Regen());
    }

    IEnumerator HitVignette()
    {
        GameManager.instance.Shake(0.1f, 0.4f);
        float startingValue = 0.3f;
        float endValue = 0.8f;
        float duration = 0;
        
        Debug.Log("Hit");
        vig.intensity.value = endValue;
        while (duration < 2f)
        {
            duration += Time.deltaTime;
            vig.intensity.value = vig.intensity.value + (startingValue - vig.intensity.value) / 200;
            yield return null;
        }
        vig.intensity.value = startingValue;
    }
}
