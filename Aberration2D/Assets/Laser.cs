using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage;
    [SerializeField] float chargeTime;
    [SerializeField] float shootTime;
    [SerializeField] float shootWidth;
    [SerializeField] float turnStrength;
    [SerializeField] GameObject sfx;
    [SerializeField] GameObject endFX;
    public GameObject enemy;
    public Vector2 shootDir;
    [SerializeField] LayerMask playerLayer;

    [Space]

    [SerializeField] LineRenderer warningLine;
    [SerializeField] LineRenderer damageLine;
    [SerializeField] UnityEngine.Rendering.Universal.Light2D laserLight;

    GameObject instant;
    bool flickering = false;

    IEnumerator Start()
    {
        float rotZ = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        laserLight.enabled = false;
        instant = Instantiate(sfx);
        
        flickering = true;
        StartCoroutine(Flicker());
        while (flickering)
        {
            Vector3 diff = Player.instance.transform.position - transform.position;
            diff.Normalize();
            shootDir = DataFragment.RotateTowards(shootDir, diff, turnStrength * Mathf.Deg2Rad, 1f);
            rotZ = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
            yield return new WaitForSeconds(1/60f);
        }
        
        damageLine.enabled = true;
        laserLight.enabled = true;
        float duration = 0;
        GameManager.instance.Shake(0.1f, 0.1f);
        while (duration < shootTime)
        {
            duration += Time.fixedDeltaTime;
            damageLine.startWidth = shootWidth * 2;
            shootWidth = shootWidth - shootWidth / 10;
            if (Physics2D.CircleCast(transform.position, shootWidth, shootDir, Mathf.Infinity, playerLayer))
            {
                Player.instance.GetComponent<PlayerHealth>().TakeDamage(damage, false);
            }
            yield return new WaitForSeconds(1/60f);
        }

        //Instantiate(endFX);
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
