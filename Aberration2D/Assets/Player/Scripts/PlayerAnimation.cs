using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public float facing = 1;

    Animator anim;
    PlayerMove pm;
    PlayerRoll pr;

    void Start()
    {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMove>();
        pr = GetComponent<PlayerRoll>();
    }

    void Update()
    {
        JumpAnimation();

        RollingAnimation();

        WalkingAnimation();

        MovementFlip();
    }

    private void JumpAnimation()
    {
        anim.SetBool("jumping", pm.jumping);
    }

    private void RollingAnimation()
    {
        anim.SetBool("rolling", pr.rolling);
    }

    private void WalkingAnimation()
    {
        anim.SetBool("walking", (pm.xInput != 0 && pm.grounded && !pr.rolling));
    }

    private void MovementFlip()
    {
        if (pm.xInput != 0)
        {
            facing = pm.xInput;
            transform.localScale = new Vector3(pm.xInput, 1, 1);
        }
    }
}
