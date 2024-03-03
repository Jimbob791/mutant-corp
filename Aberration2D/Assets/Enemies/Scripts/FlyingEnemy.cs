using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Movement Config")]
    [SerializeField] float moveSpeed;
    [SerializeField] float turnAccel;
    [SerializeField] float avoidAccel;
    [SerializeField] float distance;
    [SerializeField] float aggroDistance;

    [Space]

    [Header("Collision Checks")]
    [SerializeField] float checkDistance;
    [SerializeField] float checkAngle = 15;
    [SerializeField] LayerMask platformMask;

    [HideInInspector] public Vector3 desiredVelocity;

    GameObject player;
    Rigidbody2D rb;
    Vector3 diff;

    void Start()
    {
        player = Player.instance;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetDirection();

        ApplyVelocity();
    }

    private void GetDirection()
    {
        Vector3 target = player.transform.position - transform.position;
        
        if (Vector3.Distance(transform.position, player.transform.position) > aggroDistance)
        {
            target = Vector3.zero;
        }
        if (Vector3.Distance(transform.position, player.transform.position) < distance)
        {
            target = -target;
        }

        diff = Vector3.Lerp(diff, target, turnAccel);
        diff.Normalize();

        CheckPlatforms();
        
        desiredVelocity = Vector3.Lerp(desiredVelocity, diff, avoidAccel);
        desiredVelocity.Normalize();
        desiredVelocity *= moveSpeed;
    }

    private void CheckPlatforms()
    {
        for (int i = 0; i < 6; i++)
        {
            RaycastHit2D cast1 = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(i * checkAngle, new Vector3(0, 0, 1)) * diff, checkDistance, platformMask);
            RaycastHit2D cast2 = Physics2D.Raycast(transform.position, Quaternion.AngleAxis(-i * checkAngle, new Vector3(0, 0, 1)) * diff, checkDistance, platformMask);

            if (cast1)
            {
                Debug.DrawLine(transform.position, cast1.point, Color.green, 0.0f, false);
                diff = Quaternion.AngleAxis(-i * checkAngle, new Vector3(0, 0, 1)) * diff;
            }

            if (cast2)
            {
                Debug.DrawLine(transform.position, cast2.point, Color.green, 0.0f, false);
                diff = Quaternion.AngleAxis(i * checkAngle, new Vector3(0, 0, 1)) * diff;
            }
        } 
    }

    private void ApplyVelocity()
    {
        rb.velocity = desiredVelocity;
    }
}
