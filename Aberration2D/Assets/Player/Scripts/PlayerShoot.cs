using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Gun Config")]
    [SerializeField] float fireRate;
    [SerializeField] int magazineSize;
    [SerializeField] float reloadTime;
    [SerializeField] float bloomAngle;
    [SerializeField] Transform muzzlePoint;

    [Header("Bullet Config")]
    [SerializeField] float damage;
    [SerializeField] float bulletSpeed;
    [SerializeField] float range;
    [SerializeField] GameObject bullet;

    [Header("References")]
    [SerializeField] GameObject gun;

    PlayerAnimation pa;
    Animator anim;

    bool canShoot = true;
    bool reloading = false;
    int magazine;
    float rotZ;
    float bulletRotZ;
    Vector3 diff;

    void Start()
    {
        anim = gun.GetComponent<Animator>();
        pa = GetComponent<PlayerAnimation>();

        magazine = magazineSize;
    }

    void Update()
    {
        RotateGun();

        Shoot();
    }

    private void RotateGun()
    {
        diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gun.transform.position;
        diff.z = 0;
        diff.Normalize();  
        rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        float yFlip = diff.x > 0 ? 1 : -1;
        gun.transform.localScale = new Vector3(pa.facing, yFlip, 1);
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && canShoot && !reloading)
        {
            canShoot = false;

            anim.ResetTrigger("shoot1");
            anim.ResetTrigger("shoot2");

            Vector3 shootDir = CalculateAngle();

            GameObject newBullet = Instantiate(bullet, muzzlePoint.position, Quaternion.identity);
            newBullet.transform.rotation = Quaternion.Euler(0f, 0f, bulletRotZ);
            newBullet.GetComponent<Bullet>().desiredVelocity = shootDir * bulletSpeed;
            StartCoroutine(ResetShoot());

            if (Random.Range(-5, 5) <= 0) {
                anim.SetTrigger("shoot1");
            }
            else {
                anim.SetTrigger("shoot2");
            }

            magazine--;
            if (magazine == 0)
            {
                reloading = true;
                StartCoroutine(Reload());
            }
        }
    }

    private Vector3 CalculateAngle()
    {
        float rotAngle = Random.Range(-bloomAngle, bloomAngle);

        Vector3 dir = Quaternion.AngleAxis(rotAngle, new Vector3(0, 0, 1)) * diff;
        bulletRotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        return dir;
    }

    private IEnumerator ResetShoot()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        magazine = magazineSize;
        reloading = false;
    }
}
