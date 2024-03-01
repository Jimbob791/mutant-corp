using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] GameObject projectile;
    [SerializeField] float shotCooldown;

    bool canShoot = true;
    Animator anim;
    GameObject player;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = Player.instance;
    }

    void Update()
    {
        if (canShoot)
        {
            anim.ResetTrigger("attack");
            canShoot = false;

            anim.SetTrigger("attack");
            StartCoroutine(ResetCooldown());
        }
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(0.25f);

        Vector3 shootDir = player.transform.position - transform.position;
        shootDir.Normalize();

        GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
        shot.GetComponent<EnemyBullet>().damage = damage;
        shot.GetComponent<EnemyBullet>().dir = shootDir;

        yield return new WaitForSeconds(shotCooldown - 0.25f);
        canShoot = true;
    }
}
