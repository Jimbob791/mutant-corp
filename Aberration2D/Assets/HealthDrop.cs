using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public int healAmount;
    [SerializeField] float detectRange;
    [SerializeField] float rotSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float accel;
    [SerializeField] float maxSpeed;

    Vector3 desiredVelocity;
    bool moving = true;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!moving)
        {
            return;
        }

        Vector2 diff = Player.instance.transform.position - transform.position;
        diff.Normalize();
        desiredVelocity = DataFragment.RotateTowards(desiredVelocity, diff, rotSpeed * Mathf.Deg2Rad, 1f);

        rb.velocity = desiredVelocity * moveSpeed;
        moveSpeed += accel;
        moveSpeed = moveSpeed > maxSpeed ? maxSpeed : moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            StartCoroutine(DestroySelf());
        }
    }

    private IEnumerator DestroySelf()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        moving = false;
        Player.instance.GetComponent<PlayerHealth>().TakeDamage(-healAmount, true);

        yield return new WaitForSeconds(0.2f);
        
        Destroy(this.gameObject);
    }
}
