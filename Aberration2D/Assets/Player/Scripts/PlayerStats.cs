using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [Header("Movement Control")]
    public float moveSpeed;
    public float jumpForce;
    public float rollSpeed;

    [Header("Gun Config")]
    public float fireRate;
    public float recoil;
    public int magazineSize;
    public float reloadTime;
    public float bloomAngle;

    [Header("Health Config")]
    public int maxHealth;

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

        playerMove.moveSpeed = moveSpeed;
        playerMove.jumpForce = jumpForce;

        playerRoll.rollSpeed = rollSpeed;

        playerShoot.fireRate = fireRate;
        playerShoot.recoil = recoil;
        playerShoot.magazineSize = magazineSize;
        playerShoot.reloadTime = reloadTime;
        playerShoot.bloomAngle = bloomAngle;

        playerhealth.maxHealth = maxHealth;

        playerShoot.damage = damage;
        playerShoot.homingStrength = homingStrength;
        playerShoot.selfDamage = selfDamage;
        playerShoot.bulletSpeed = bulletSpeed;
        playerShoot.range = range;
    }
}
