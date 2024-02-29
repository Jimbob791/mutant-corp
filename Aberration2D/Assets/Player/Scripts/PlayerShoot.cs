using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] GameObject gun;

    PlayerAnimation pa;

    void Start()
    {
        pa = GetComponent<PlayerAnimation>();
    }

    void Update()
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gun.transform.position;
        diff.Normalize();  
        float rotz = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.Euler(0f, 0f, rotz);

        float yFlip = diff.x > 0 ? 1 : -1;
        gun.transform.localScale = new Vector3(pa.facing, yFlip, 1);
    }
}
