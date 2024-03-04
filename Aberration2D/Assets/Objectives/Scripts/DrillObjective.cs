using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrillObjective : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Slider slider;
    [SerializeField] float timeToActivate;

    float progress = 0;
    bool active = false;
    public bool complete = false;

    void Update()
    {
        if (complete)
        {
            return;
        }

        if (active)
        {
            progress += Time.deltaTime;

            if (progress > timeToActivate)
            {
                complete = true;
                return;
            }

            slider.value = progress / timeToActivate;
        }
        else if (Input.GetKeyDown(KeyCode.E) && Physics2D.BoxCast(transform.position, new Vector2(1.5f, 1.5f), 0, Vector2.zero, 0, playerLayer))
        {
            active = true;
            GetComponent<Animator>().enabled = true;
        }
    }
}
