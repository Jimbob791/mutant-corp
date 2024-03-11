using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class C4 : MonoBehaviour
{
    public GameObject enemy;
    public int damage;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject deathSFX;
    [SerializeField] GameObject beepSFX;
    [SerializeField] Light2D light;
    [SerializeField] Sprite off;
    [SerializeField] Sprite on;

    IEnumerator Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        for (int i = 0; i < 4; i++)
        {
            sr.sprite = on;
            light.enabled = true;
            Instantiate(beepSFX);
            yield return new WaitForSeconds(0.25f);
            sr.sprite = off;
            light.enabled = false;
            yield return new WaitForSeconds(0.25f);
        }

        Instantiate(explosion, transform.position, Quaternion.identity);
        Instantiate(deathSFX);
        
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 3, Vector2.zero, 0, enemyLayer);
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].transform.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage, true);
        }
        GameManager.instance.Shake(0.05f, 0.05f);
        Destroy(this.gameObject);
    }

    void Update()
    {
        if (enemy != null) transform.position = enemy.transform.position;
    }
}
