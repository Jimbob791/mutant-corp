using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrillObjective : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Slider slider;
    [SerializeField] float timeToActivate;
    [SerializeField] ParticleSystem dust;

    float progress = 0;
    bool active = false;
    public bool complete = false;
    Vector3 pos;

    void Start()
    {
        dust.Stop();
        pos = transform.position;
    }

    void Update()
    {
        if (complete)
        {
            return;
        }

        if (active)
        {
            progress += Time.deltaTime;
            transform.localPosition = pos + new Vector3(Random.Range(-0.05f, 0.05f), 0, 0);

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
            dust.Play();
            GetComponent<Animator>().enabled = true;
        }
    }
}
