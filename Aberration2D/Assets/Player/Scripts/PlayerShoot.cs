using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    [Header("Gun Config")]
    public float fireRate;
    public float recoil;
    public int magazineSize;
    public int burstSize;
    public float burstDelay;
    public float reloadTime;
    public float bloomAngle;
    [SerializeField] Transform muzzlePoint;

    [Header("Bullet Config")]
    public int damage;
    public float homingStrength;
    public int selfDamage;
    public float bulletSpeed;
    public float range;
    public float damagePerData;
    public GameObject bullet;
    public GameObject c4;
    public GameObject grenade;
    public GameObject javelin;
    public GameObject drone;
    public int droneStacks;

    [Header("References")]
    [SerializeField] GameObject gun;
    [SerializeField] GameObject reloadBar;
    [SerializeField] GameObject reloadProgress;
    [SerializeField] GameObject quickShotSFX;
    [SerializeField] GameObject mediumShotSFX;
    [SerializeField] GameObject longShotSFX;
    [SerializeField] GameObject reloadSFX;

    PlayerAnimation pa;
    PlayerRoll pr;
    Animator anim;
    SpriteRenderer gunRenderer;

    bool canShoot = true;
    bool reloading = false;
    public int magazine;
    float rotZ;
    float bulletRotZ;
    Vector3 diff;
    public int numSpawned = 0;
    int numDrones;
    public List<GameObject> drones = new List<GameObject>();

    void Start()
    {
        anim = gun.GetComponent<Animator>();
        pa = GetComponent<PlayerAnimation>();
        pr = GetComponent<PlayerRoll>();
        gunRenderer = gun.transform.GetChild(0).GetComponent<SpriteRenderer>();

        magazine = magazineSize;
        numDrones = droneStacks * 2;

        StartCoroutine(SummonDrone());
    }

    void Update()
    {
        RollingCheck();

        RotateGun();

        Shoot();

        Drones();
    }

    private void RollingCheck()
    {
        gunRenderer.enabled = !pr.rolling;
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
        if (Input.GetMouseButton(0) && canShoot && !reloading && !pr.rolling)
        {
            GetComponent<PlayerItems>().Shoot();
            if (burstSize == 1)
            {
                canShoot = false;

                anim.ResetTrigger("shoot1");
                anim.ResetTrigger("shoot2");

                Vector3 shootDir = CalculateAngle();

                GameObject newBullet = Instantiate(bullet, muzzlePoint.position, Quaternion.identity);
                newBullet.transform.rotation = Quaternion.Euler(0f, 0f, bulletRotZ);
                newBullet.GetComponent<Bullet>().desiredVelocity = shootDir;
                newBullet.GetComponent<Bullet>().damage = damage;
                newBullet.GetComponent<Bullet>().speed = bulletSpeed;
                newBullet.GetComponent<Bullet>().homingStrength = homingStrength;
                Destroy(newBullet, range);
                StartCoroutine(ResetShoot());

                if (selfDamage != 0)
                    GetComponent<PlayerHealth>().TakeDamage(selfDamage, true);

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

                if (fireRate < 0.25)
                {
                    Instantiate(quickShotSFX);
                }
                else if (fireRate < 1.5f)
                {
                    Instantiate(mediumShotSFX);
                }
                else
                {
                    Instantiate(longShotSFX);
                }
            }
            else
            {
                canShoot = false;
                StartCoroutine(Burst(burstSize, burstDelay));
            }
        }
    }

    IEnumerator Burst(int num, float delay)
    {
        if (burstDelay == 0)
        {
            Instantiate(longShotSFX);
        }

        for (int i = 0; i < num; i++)
        {
            anim.ResetTrigger("shoot1");
            anim.ResetTrigger("shoot2");

            Vector3 shootDir = CalculateAngle();

            GameObject newBullet = Instantiate(bullet, muzzlePoint.position, Quaternion.identity);
            newBullet.transform.rotation = Quaternion.Euler(0f, 0f, bulletRotZ);
            newBullet.GetComponent<Bullet>().desiredVelocity = shootDir;
            newBullet.GetComponent<Bullet>().damage = damage + Mathf.RoundToInt(GetComponent<PlayerData>().data * damagePerData);
            newBullet.GetComponent<Bullet>().speed = bulletSpeed;
            newBullet.GetComponent<Bullet>().homingStrength = homingStrength;
            Destroy(newBullet, range);

            if (selfDamage != 0)
                GetComponent<PlayerHealth>().TakeDamage(selfDamage, true);

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
                StartCoroutine(ResetShoot());
                yield break;
            }

            if (burstDelay != 0)
            {
                Instantiate(quickShotSFX);
            }

            yield return new WaitForSeconds(delay);
        }

        StartCoroutine(ResetShoot());
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
        reloadBar.GetComponent<Image>().enabled = true;
        reloadProgress.GetComponent<Image>().enabled = true;
        GetComponent<PlayerItems>().Reload();
        StartCoroutine(ResetReload());
        float duration = 0;

        while (duration < reloadTime)
        {
            duration += Time.deltaTime;

            reloadProgress.transform.localPosition = new Vector3(-40.5f + (duration / reloadTime) * 81, 0, 0);

            yield return null;
        }
    }

    private IEnumerator ResetReload()
    {
        yield return new WaitForSeconds(reloadTime);
        FinishReload();
    }

    public void FinishReload()
    {
        Instantiate(reloadSFX);
        magazine = magazineSize;
        reloading = false;
        reloadBar.GetComponent<Image>().enabled = false;
        reloadProgress.GetComponent<Image>().enabled = false;
    }

    public void ShootGrenade()
    {
        Vector3 shootDir = CalculateAngle();

        GameObject newBullet = Instantiate(grenade, muzzlePoint.position, Quaternion.identity);
        newBullet.transform.rotation = Quaternion.Euler(0f, 0f, bulletRotZ);
        newBullet.GetComponent<Bullet>().desiredVelocity = shootDir;
        newBullet.GetComponent<Bullet>().damage = damage * 5 + Mathf.RoundToInt(GetComponent<PlayerData>().data * damagePerData);
        newBullet.GetComponent<Bullet>().speed = bulletSpeed / 1.5f;
        newBullet.GetComponent<Bullet>().homingStrength = 0;
        Destroy(newBullet, 60f);
    }

    public void ShootJavelin()
    {
        Vector3 shootDir = new Vector3(0, 1, 0);
        shootDir = Quaternion.AngleAxis(Random.Range(-40.5f, 40.5f), new Vector3(0, 0, 1)) * shootDir;
        bulletRotZ = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        GameObject newBullet = Instantiate(javelin, muzzlePoint.position, Quaternion.identity);
        newBullet.transform.rotation = Quaternion.Euler(0f, 0f, bulletRotZ);
        newBullet.GetComponent<Bullet>().desiredVelocity = shootDir;
        newBullet.GetComponent<Bullet>().damage = damage * 2 + Mathf.RoundToInt(GetComponent<PlayerData>().data * damagePerData);
        Destroy(newBullet, 60f);
    }

    public void SpawnC4(GameObject enemy, int damage)
    {
        GameObject newBullet = GameObject.Instantiate(c4, enemy.transform.position, Quaternion.identity);
        newBullet.GetComponent<C4>().damage = damage;
        newBullet.GetComponent<C4>().enemy = enemy;
        GameObject.Destroy(newBullet, 60f);
    }

    private IEnumerator SummonDrone()
    {
        if (droneStacks == 0)
        {
            yield return new WaitForSeconds(2);
        }
        else
        {
            yield return new WaitForSeconds(8 / droneStacks);
        }
        
        if (numSpawned < numDrones)
        {
            GameObject newDrone = Instantiate(drone, transform.position, Quaternion.identity);
            newDrone.GetComponent<DroneMovement>().damage = droneStacks * 30;
            numSpawned += 1;
            drones.Add(newDrone);
            for(int i = 0; i < drones.Count; i++)
            {
                drones[i].GetComponent<DroneMovement>().offset = 0;
            }
        }
        StartCoroutine(SummonDrone());
    }

    private void Drones()
    {
        numDrones = droneStacks * 3;
        for(int i = 0; i < drones.Count; i++)
        {
            drones[i].GetComponent<DroneMovement>().index = i;
            drones[i].GetComponent<DroneMovement>().numDrones = numDrones;
        }
    }
}
