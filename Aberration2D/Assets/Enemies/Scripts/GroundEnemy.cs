using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    [SerializeField] Transform holeCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] Transform groundCheck;
    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;
    [SerializeField] float aggroDistance;
    [SerializeField] float gravity;
    [SerializeField] LayerMask platformMask;

    Rigidbody2D rb;
    Animator anim;
    [HideInInspector] public bool aggro;
    bool grounded;
    [HideInInspector] public bool attacking;
    public Vector3 desiredVelocity;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        desiredVelocity.y -= gravity;
        MoveToPlayer();
        WallCheck();
        HoleCheck();
        SetVelocity();
    }

    private void MoveToPlayer()
    {
        if (Vector3.Distance(Player.instance.transform.position, transform.position) > aggroDistance || attacking)
        {
            aggro = false;
            desiredVelocity.x = 0;
            anim.SetBool("walking", false);
            return;
        }
        aggro = true;

        desiredVelocity.x = Player.instance.transform.position.x < transform.position.x ? -1 : 1;
        desiredVelocity.x *= moveSpeed;
        anim.SetBool("walking", true);
    }

    private void HoleCheck()
    {
        if (desiredVelocity.x == GetComponent<EnemyFlip>().targetFlip)
        {
            // facing wrong way
        }
        else if (!Physics2D.BoxCast(holeCheck.position, new Vector2(0.1f, 0.5f), 0, Vector2.zero, 0, platformMask))
        {
            desiredVelocity.x = 0;
            anim.SetBool("walking", false);
        }
    }

    private void WallCheck()
    {
        grounded = Physics2D.BoxCast(groundCheck.position, new Vector2(1f, 0.1f), 0, Vector2.zero, 0, platformMask);
        if (!aggro)
            return;
        if (Physics2D.BoxCast(wallCheck.position, new Vector2(0.1f, 0.1f), 0, Vector2.zero, 0, platformMask))
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (grounded)
            desiredVelocity.y = jumpForce;
    }

    private void SetVelocity()
    {
        rb.velocity = desiredVelocity;
    }
}
