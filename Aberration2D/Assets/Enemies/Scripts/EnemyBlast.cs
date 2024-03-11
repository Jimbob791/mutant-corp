using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlast : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] GameObject projectile;
    [SerializeField] float shotCooldown;
    [SerializeField] int numShots;
    [SerializeField] GameObject blastSFX;

    bool canShoot = true;
    Animator anim;
    GameObject player;
    Vector3 shootDir;

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

        Instantiate(blastSFX);

        for (int i = 0; i < numShots; i++)
        {
            float rotZ = (360 / numShots) * i;
            shootDir = Quaternion.AngleAxis(rotZ, new Vector3(0, 0, 1)) * new Vector3(0, 1, 0);

            GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
            shot.GetComponent<EnemyBullet>().damage = damage;
            shot.GetComponent<EnemyBullet>().dir = shootDir;
        }

        yield return new WaitForSeconds(shotCooldown - 0.25f);
        canShoot = true;
    }
}
