using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBig : MonoBehaviour
{
    public int damage;
    [SerializeField] float chargeTime;
    [SerializeField] float shootTime;
    [SerializeField] float shootWidth;
    [SerializeField] float turnStrength;
    [SerializeField] float flickerRate;
    [SerializeField] float coolTime;
    [SerializeField] GameObject sfx;
    [SerializeField] GameObject endFX;
    public GameObject enemy;
    public Vector2 shootDir;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] LayerMask enemyLayer;

    [Space]

    [SerializeField] LineRenderer warningLine;
    [SerializeField] LineRenderer damageLine;
    [SerializeField] UnityEngine.Rendering.Universal.Light2D laserLight;

    GameObject instant;
    bool flickering = false;

    IEnumerator Start()
    {   
        float startVolume = GameObject.Find("MainMusic").GetComponent<AudioSource>().volume;
        for (int i = 0; i < 60; i++)
        {
            GameObject.Find("MainMusic").GetComponent<AudioSource>().volume -= startVolume / 60;
            yield return new WaitForSeconds(1/60f);
        }
        GameObject.Find("MainMusic").GetComponent<AudioSource>().volume = 0;
        float rotZ = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        laserLight.enabled = false;
        instant = Instantiate(sfx);
        
        flickering = true;
        StartCoroutine(Flicker());
        while (flickering)
        {
            Rotate();
            yield return new WaitForSeconds(1/60f);
        }
        
        damageLine.enabled = true;
        laserLight.enabled = true;
        float duration = 0;
        GameManager.instance.Shake(shootTime + 0.4f, 0.4f);
        float lastScan = 0;
        while (duration < shootTime)
        {
            duration += Time.fixedDeltaTime;
            lastScan += (1/60);
            
            damageLine.startWidth = shootWidth;
            int change = (int)Mathf.Floor(duration * 8) % 2 == 0 ? -1 : 1 ;
            shootWidth += flickerRate * change;

            if (Physics2D.CircleCast(transform.position, shootWidth / 2, shootDir, Mathf.Infinity, playerLayer))
            {
                Player.instance.GetComponent<PlayerHealth>().TakeDamage(damage, false);
            }
            if (lastScan > 0.3f)
            {
                lastScan = 0;
                RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, shootWidth / 2, shootDir, Mathf.Infinity, enemyLayer);
                if (hit.Length != 0)
                {
                    for (int i = 0; i < hit.Length; i++)
                    {
                        if (hit[i].transform.gameObject != enemy)
                        {
                            hit[i].transform.gameObject.GetComponent<EnemyHealth>().TakeDamage(20, true);
                        }
                    }
                }
            }

            Rotate();

            yield return new WaitForSeconds(1/60f);
        }

        float endWidth = shootWidth;
        for (int i = 0; i < 10; i++)
        {
            damageLine.startWidth = shootWidth;
            shootWidth -= endWidth / 10;

            yield return new WaitForSeconds(1/60f);
        }
        damageLine.enabled = false;
        laserLight.enabled = false;

        for (int i = 0; i < 60; i++)
        {
            GameObject.Find("MainMusic").GetComponent<AudioSource>().volume += startVolume / 60;
            yield return new WaitForSeconds(1/60f);
        }

        Destroy(this.gameObject);
    }

    void Update()
    {
        if (enemy != null)
            transform.position = enemy.transform.position;
        else
        {
            Destroy(instant);
            Destroy(this.gameObject);
        }
    }

    void Rotate()
    {
        Vector3 diff = Player.instance.transform.position - transform.position;
        diff.Normalize();
        shootDir = DataFragment.RotateTowards(shootDir, diff, turnStrength * Mathf.Deg2Rad, 1f);
        float rotZ = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }

    IEnumerator Flicker()
    {
        for (int i = 0; i < 10; i++)
        {
            warningLine.enabled = true;
            yield return new WaitForSeconds(chargeTime / 20);
            warningLine.enabled = false;
            yield return new WaitForSeconds(chargeTime / 20);
        }
        flickering = false;
    }
}
