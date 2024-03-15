using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [Header("Movement Control")]
    public float moveSpeed;
    public int numJumps;
    public float jumpForce;
    public float rollSpeed;

    [Header("Gun Config")]
    public float fireRate;
    public float recoil;
    public int magazineSize;
    public int burstSize;
    public float burstDelay;
    public float reloadTime;
    public float bloomAngle;
    public float damagePerData;
    public int droneStacks;

    [Header("Health Config")]
    public int maxHealth;
    public int lifeSteal;
    public int regen;
    public int lives;

    [Header("Bullet Config")]
    public int damage;
    public float homingStrength;
    public int selfDamage;
    public float bulletSpeed;
    public float range;

    GameObject player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetPlayerStats()
    {
        player = Player.instance;
        PlayerShoot playerShoot = player.GetComponent<PlayerShoot>();
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        PlayerHealth playerhealth = player.GetComponent<PlayerHealth>();
        PlayerRoll playerRoll = player.GetComponent<PlayerRoll>();

        if (moveSpeed < 0)
            moveSpeed = 0;
        if (jumpForce < 0)
            jumpForce = 0;
        if (fireRate < 0)
            fireRate = 0;
        if (magazineSize < 1)
            magazineSize = 1;
        if (maxHealth < 1)
            maxHealth = 1;
        if (lifeSteal < 0)
            lifeSteal = 0;
        if (damage < 1)
            damage = 1;
        if (selfDamage < 0)
            selfDamage = 0;
        if (bulletSpeed < 1)
            bulletSpeed = 1;
        if (range < 0.1f)
            range = 0.1f;
        if (bloomAngle < 0)
            bloomAngle = 0;
        if (reloadTime < fireRate)
            reloadTime = fireRate;

        playerMove.moveSpeed = moveSpeed;
        playerMove.jumpForce = jumpForce;
        playerMove.numJumps = numJumps;

        playerRoll.rollSpeed = rollSpeed;

        playerShoot.fireRate = fireRate;
        playerShoot.recoil = recoil;
        playerShoot.magazineSize = magazineSize;
        playerShoot.burstSize = burstSize;
        playerShoot.burstDelay = burstDelay;
        playerShoot.reloadTime = reloadTime;
        playerShoot.bloomAngle = bloomAngle;
        playerShoot.damagePerData = damagePerData;
        playerShoot.droneStacks = droneStacks;

        playerhealth.maxHealth = maxHealth;
        playerhealth.lifeSteal = lifeSteal;
        playerhealth.regen = regen;
        playerhealth.SetHealth();
        playerhealth.lives = lives;

        playerShoot.damage = damage;
        playerShoot.homingStrength = homingStrength;
        playerShoot.selfDamage = selfDamage;
        playerShoot.bulletSpeed = bulletSpeed;
        playerShoot.range = range;
    }
}
