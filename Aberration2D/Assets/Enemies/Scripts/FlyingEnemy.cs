using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Movement Config")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] float distance;
    [SerializeField] float aggroDistance;

    [Space]

    [Header("Collision Checks")]
    [SerializeField] float checkDistance;
    [SerializeField] LayerMask platformMask;

    [HideInInspector] public Vector2 desiredVelocity = new Vector2(0, 1);

    Rigidbody2D rb;
    Vector2 diff;
    Vector2 sums;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        GetDirection();

        ApplyVelocity();
    }

    private void GetDirection()
    {
        Vector2 diff = Player.instance.transform.position - transform.position;
        diff.Normalize();

        float dist = Vector3.Distance(Player.instance.transform.position, transform.position);

        if (dist < distance)
        {
            diff = -diff;
        }
        if (dist > aggroDistance)
        {
            diff = Vector3.zero;
        }

        diff = CheckPlatforms(diff).normalized;

        desiredVelocity = DataFragment.RotateTowards(desiredVelocity, diff, rotSpeed, 1);
    }

    private Vector2 CheckPlatforms(Vector2 diff)
    {
        sums = diff;
        Vector2 starting = diff;

        for (int i = 0; i < 8; i++)
        {
            Vector2 castDir = Quaternion.AngleAxis(i * 15, new Vector3(0, 0, 1)) * starting;
            if (Physics2D.Raycast(transform.position, castDir, checkDistance, platformMask))
            {
                sums += -castDir;
            }
            else
            {
                sums += castDir;
            }

            castDir = Quaternion.AngleAxis(i * -15, new Vector3(0, 0, 1)) * starting;
            if (Physics2D.Raycast(transform.position, castDir, checkDistance, platformMask))
            {
                sums += -castDir;
            }
            else
            {
                sums += castDir;
            }
        }
        sums.Normalize();

        return sums;
    }

    private void ApplyVelocity()
    {
        rb.velocity = desiredVelocity * moveSpeed;
    }
}
