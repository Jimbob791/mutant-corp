using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Movement Control")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Gun Config")]
    public float fireRate;
    public float recoil;
    public int magazineSize;
    public float reloadTime;
    public float bloomAngle;

    [Header("Bullet Config")]
    public int damage;
    public float bulletSpeed;
    public float range;
}
