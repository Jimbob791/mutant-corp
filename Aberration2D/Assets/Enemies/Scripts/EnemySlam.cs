using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlam : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] int damage;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform attackPos;

    Animator anim;
    GroundEnemy ground;
    float attackTime;
    float attackCooldown = 4;
    int numProjectiles = 8;
    Vector3 shootDir;

    void Start()
    {
        anim = GetComponent<Animator>();
        ground = GetComponent<GroundEnemy>();
    }

    void Update()
    {
        if (!ground.aggro)
        {
            return;
        }
        attackTime += Time.deltaTime;

        if ((attackTime > attackCooldown || Vector3.Distance(Player.instance.transform.position, attackPos.position) < 2) && !ground.attacking)
        {
            attackTime = 0;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        ground.attacking = true;
        anim.SetBool("attacking", true);
        yield return new WaitForSeconds(0.7f);
        anim.SetBool("attacking", false);
        if (Physics2D.BoxCast(attackPos.position, new Vector2(0.5f, 0.5f), 0, Vector2.zero, 0, playerLayer))
        {
            Player.instance.GetComponent<PlayerHealth>().TakeDamage(damage, false);
        }
        GameManager.instance.Shake(0.05f, 0.1f);
        Vector3 originalDir = new Vector3(-2, 1, 0);
        originalDir.Normalize();
        for (int i = 0; i < numProjectiles; i++)
        {
            float rotZ = Random.Range(-20, 30);
            shootDir = Quaternion.AngleAxis(rotZ, new Vector3(0, 0, 1)) * originalDir;
            GameObject shot = Instantiate(projectile, attackPos.position, Quaternion.identity);
            shot.GetComponent<EnemyBullet>().damage = damage;
            shot.GetComponent<EnemyBullet>().dir = new Vector3(shootDir.x * GetComponent<EnemyFlip>().targetFlip, shootDir.y, 0);
        }
        yield return new WaitForSeconds(0.5f);
        ground.attacking = false;
    }
}
