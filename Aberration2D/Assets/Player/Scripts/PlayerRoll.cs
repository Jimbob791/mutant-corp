using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoll : MonoBehaviour
{
    [Header("Movement Control")]
    public float rollSpeed;
    [SerializeField] float rollTime;

    [HideInInspector] public bool rolling;

    Rigidbody2D rb;
    PlayerMove pm;

    bool rollDesired;
    float rollInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pm = GetComponent<PlayerMove>();
    }

    void Update()
    {
        GetInput();

        Roll();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !rolling && !rollDesired)
        {
            rollDesired = true;
        }
    }

    private void Roll()
    {
        if (rollDesired)
        {
            rollInput = GetComponent<PlayerAnimation>().facing;
            rollDesired = false;
            rolling = true;
            StartCoroutine(EndRoll());
        }

        if (rolling)
        {
            pm.desiredVelocity.x = rollSpeed * rollInput;
        }

        pm.canMove = !rolling;
    }

    private IEnumerator EndRoll()
    {
        yield return new WaitForSeconds(rollTime);
        rolling = false;
    }
}
