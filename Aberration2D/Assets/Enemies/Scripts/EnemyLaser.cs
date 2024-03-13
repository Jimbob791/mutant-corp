using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public int damage;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float attackRange;
    [SerializeField] float cooldown;
    [SerializeField] Transform shootPos;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(2, 5));

        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        bool inRange = false;
        while (!inRange)
        {
            inRange = Vector3.Distance(Player.instance.transform.position, transform.position) < attackRange;
            yield return null;
        }

        GameObject laser = Instantiate(laserPrefab, shootPos.localPosition, Quaternion.identity);
        if (laser.GetComponent<Laser>() != null)
        {
            laser.GetComponent<Laser>().damage = damage;
            laser.GetComponent<Laser>().offset = shootPos.localPosition;
            Vector3 diff = Player.instance.transform.position - transform.position;
            diff.Normalize();
            laser.GetComponent<Laser>().shootDir = diff;
            laser.GetComponent<Laser>().enemy = this.gameObject;
        }
        else if (laser.GetComponent<LaserBig>() != null)
        {
            laser.GetComponent<LaserBig>().damage = damage;
            Vector3 diff = Player.instance.transform.position - transform.position;
            diff.Normalize();
            laser.GetComponent<LaserBig>().shootDir = diff;
            laser.GetComponent<LaserBig>().enemy = this.gameObject;
        }
        yield return new WaitForSeconds(cooldown);
        StartCoroutine(Shoot());
    }
}
