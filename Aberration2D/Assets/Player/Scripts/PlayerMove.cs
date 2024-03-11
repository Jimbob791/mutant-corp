using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement Control")]
    public float moveSpeed;
    [SerializeField] float groundAccel;
    [SerializeField] float airAccel;
    public float jumpForce;
    public int numJumps;
    [SerializeField] float slowDown;
    [SerializeField] float gravity;

    [Space]

    [Header("QoL Control")]
    [SerializeField] float coyoteTime;
    [SerializeField] float bufferTime;

    [Space]

    [Header("Collision")]
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform ceilingCheck;
    [SerializeField] Transform leftCheck;
    [SerializeField] Transform rightCheck;
    [SerializeField] Vector2 checkSize;
    [SerializeField] LayerMask platformMask;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public float xInput;
    [HideInInspector] public bool grounded;
    [HideInInspector] public bool jumping;
    [HideInInspector] public Vector3 desiredVelocity;
    
    Rigidbody2D rb;

    float timeSinceGrounded;
    float timeSinceJump;
    bool forcedJump = false;
    bool groundedLastFrame;
    int usedJumps = 0;
    Vector3 stepGround = new Vector3(0.2f, -0.45f, 0);
    Vector3 stepAir = new Vector3(0.2f, 0.05f, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        IterateTimers();

        GetInput();

        DetectGround();

        DetectCeiling();
    }

    void FixedUpdate()
    {
        ModifyInput();

        JumpSettings();

        SetVelocity();
    }

    private void IterateTimers()
    {
        timeSinceGrounded += Time.deltaTime;
        timeSinceJump += Time.deltaTime;
    }

    private void DetectGround()
    {
        grounded = Physics2D.BoxCast(groundCheck.position, checkSize, 0, Vector2.zero, 0, platformMask);

        if (grounded)
        {
            if (Physics2D.BoxCast(new Vector3(transform.position.x + stepGround.x * xInput, transform.position.y + stepGround.y, 0), new Vector2(0.05f, 0.05f), 0, Vector2.zero, 0, platformMask))
            {
                if (!Physics2D.BoxCast(new Vector3(transform.position.x + stepAir.x * xInput, transform.position.y + stepAir.y, 0), new Vector2(0.05f, 0.05f), 0, Vector2.zero, 0, platformMask))
                {
                    if (xInput != 0)
                    {
                        transform.position = transform.position + new Vector3(0, 0.55f, 0);
                    }
                }
            }
        }
    }

    private void DetectCeiling()
    {
        if (Physics2D.BoxCast(ceilingCheck.position, checkSize, 0, Vector2.zero, 0, platformMask) && rb.velocity.y >= 0)
        {
            desiredVelocity.y = 0;
        }
    }

    private void GetInput()
    {
        xInput = Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A));
        timeSinceJump = Input.GetKeyDown(KeyCode.W) ? 0 : timeSinceJump;

        if (!canMove)
        {
            xInput = 0;
            timeSinceJump = 999;
        }
    }

    private void ModifyInput()
    {
        float accel = grounded ? groundAccel : airAccel;

        if (!GetComponent<PlayerRoll>().rolling)
        {
            desiredVelocity.x = desiredVelocity.x + (xInput * moveSpeed - desiredVelocity.x) * accel;
        }

        desiredVelocity.y -= gravity;

        if (grounded && !forcedJump)
        {
            timeSinceGrounded = 0;
            jumping = false;
            usedJumps = 0;
            desiredVelocity.y = 0;
        }
    }

    private void JumpSettings()
    {
        if (jumping)
        {
            if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.W))
            {
                if (!forcedJump)
                    desiredVelocity.y *= slowDown;
            }
        }

        if (grounded && timeSinceJump <= bufferTime)
        {
            Jump(1, false);
            
        }
        else if (!grounded && rb.velocity.y < 0 && timeSinceGrounded <= coyoteTime && timeSinceJump <= bufferTime)
        {
            Jump(1, false);
        }
        else if (!grounded && usedJumps < numJumps && timeSinceJump <= bufferTime)
        {
            Jump(1, false);
            usedJumps += 1;
        }
    }

    public void Jump(float force, bool forced)
    {
        forcedJump = forced;
        timeSinceJump = 999;
        jumping = true;
        desiredVelocity.y = jumpForce * force;
        StartCoroutine(ResetForced());
    }

    IEnumerator ResetForced()
    {
        yield return new WaitForSeconds(0.2f);
        forcedJump = false;
    }

    private void SetVelocity()
    {
        rb.velocity = desiredVelocity;
    }
}
