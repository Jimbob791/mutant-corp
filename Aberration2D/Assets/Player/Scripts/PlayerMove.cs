using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement Control")]
    [SerializeField] float moveSpeed;
    [SerializeField] float groundAccel;
    [SerializeField] float airAccel;
    [SerializeField] float jumpForce;
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

        if (grounded)
        {
            timeSinceGrounded = 0;
            jumping = false;
            desiredVelocity.y = 0;
        }
    }

    private void JumpSettings()
    {
        if (jumping)
        {
            if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.W))
            {
                desiredVelocity.y *= slowDown;
            }
        }

        if (grounded && timeSinceJump <= bufferTime)
        {
            Jump();
        }
        if (!grounded && rb.velocity.y < 0 && timeSinceGrounded <= coyoteTime && timeSinceJump <= bufferTime)
        {
            Jump();
        }
    }

    private void Jump()
    {
        timeSinceJump = 999;
        jumping = true;
        desiredVelocity.y = jumpForce;
    }

    private void SetVelocity()
    {
        rb.velocity = desiredVelocity;
    }
}
