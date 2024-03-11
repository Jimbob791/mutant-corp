using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour
{
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
}
