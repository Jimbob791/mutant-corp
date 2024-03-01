using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlip : MonoBehaviour
{
    [SerializeField] float zeroRange;
    [SerializeField] string type = "Flying";

    FlyingEnemy flying;

    float targetFlip = 1;

    void Start()
    {
        flying = GetComponent<FlyingEnemy>() != null ? GetComponent<FlyingEnemy>() : null;
    }

    void Update()
    {
        if (type == "Flying")
        {
            if (Mathf.Abs(flying.desiredVelocity.x) > zeroRange)
            {
                targetFlip = flying.desiredVelocity.x > 0 ? 1 : -1;
            }
        }

        transform.localScale = new Vector3(targetFlip, 1, 1);
    }
}
